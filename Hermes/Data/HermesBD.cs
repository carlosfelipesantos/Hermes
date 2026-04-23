using Hermes.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Data
{
    public class HermesBD : DbContext
    {
        public HermesBD(DbContextOptions<HermesBD> options) : base(options)
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Transportador> Transportadores { get; set; }
        public DbSet<Frete> Fretes { get; set; }

        public DbSet<DisponibilidadeBase> DisponibilidadesBase { get; set; }

        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<Notificacao> Notificacoes { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            //Herança TPH (Table Per Hierarchy) para Usuario, Cliente e Transportador
            modelBuilder.Entity<Usuario>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<Usuario>("Usuario")
                .HasValue<Cliente>("Cliente")
                .HasValue<Transportador>("Transportador");

            //Cliente -> Transportador 1:N (FretesSolicitados)
            modelBuilder.Entity<Frete>()
                .HasOne(f => f.Cliente)
                .WithMany(c => c.FretesSolicitados)
                .HasForeignKey(f => f.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            //Transportador -> Frete 1:N (FretesAceitos)
            modelBuilder.Entity<Frete>()
                .HasOne(f => f.Transportador)
                .WithMany(t => t.FretesAceitos)
                .HasForeignKey(f => f.TransportadorId)
                .OnDelete(DeleteBehavior.Restrict);

            //Transportador -> Veiculo 1:N
            modelBuilder.Entity<Veiculo>()
              .HasOne(v => v.Transportador)
              .WithMany(t => t.Veiculos)
              .HasForeignKey(v => v.TransportadorId)
              .OnDelete(DeleteBehavior.Cascade);

            //Frete -> Avaliacao 1:1
            modelBuilder.Entity<Frete>()
              .HasOne(f => f.Avaliacao)
              .WithOne(a => a.Frete)
              .HasForeignKey<Avaliacao>(a => a.FreteId);

            modelBuilder.Entity<Avaliacao>()
                .HasOne(a => a.Cliente)
                .WithMany()
                .HasForeignKey(a => a.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            //Usuario -> Notificacao 1:N
            modelBuilder.Entity<Notificacao>()
               .HasOne(n => n.Usuario)
               .WithMany(u => u.Notificacoes)
               .HasForeignKey(n => n.UsuarioId)
               .OnDelete(DeleteBehavior.Cascade);

            //Frete -> Notificacao 1:N
            modelBuilder.Entity<Notificacao>()
                .HasOne(n => n.Frete)
                .WithMany()
                .HasForeignKey(n => n.FreteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(u => u.DDD).HasMaxLength(5);   // DDD tem no máximo 5 caracteres
                entity.Property(u => u.Telefone).HasMaxLength(15); 

                // Índice único composto
                entity.HasIndex(u => new { u.DDD, u.Telefone }).IsUnique();
            });

            // Índice único para Email
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Índice único para Telefone + DDD
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => new { u.DDD, u.Telefone })
                .IsUnique();


            modelBuilder.Entity<DisponibilidadeBase>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.DiaSemana).IsRequired();
                entity.Property(e => e.HoraInicio).IsRequired();
                entity.Property(e => e.HoraFim).IsRequired();

                entity.HasOne(d => d.Transportador)
                      .WithMany(t => t.Disponibilidades) // Certifique-se que Transportador tenha ICollection<DisponibilidadeBase>
                      .HasForeignKey(d => d.TransportadorId)
                      .OnDelete(DeleteBehavior.Cascade);
            });


            // Configurações para propriedades decimal,valor e nota
            modelBuilder.Entity<Frete>()
              .Property(f => f.Valor)
              .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Avaliacao>()
                .Property(a => a.Nota)
                .HasColumnType("decimal(3,2)");



            //  ÍNDICES PARA MELHORAR PERFORMANCE

            // Para busca de fretes por status e data (mais usado)
            modelBuilder.Entity<Frete>()
                .HasIndex(f => new { f.Status, f.DataSolicitacao })
                .HasDatabaseName("IX_Fretes_Status_DataSolicitacao");

            // Para busca de fretes disponíveis (TransportadorId = null)
            modelBuilder.Entity<Frete>()
                .HasIndex(f => new { f.TransportadorId, f.Status })
                .HasDatabaseName("IX_Fretes_TransportadorId_Status");

            // Para busca de fretes por cidade de origem
            modelBuilder.Entity<Frete>()
                .HasIndex(f => f.CidadeOrigem)
                .HasDatabaseName("IX_Fretes_CidadeOrigem");

            // Para busca de janelas de disponibilidade (mais usado nas consultas de agenda)
            modelBuilder.Entity<DisponibilidadeBase>()
                .HasIndex(d => new { d.TransportadorId, d.DiaSemana })
                .HasDatabaseName("IX_DisponibilidadesBase_TransportadorId_DiaSemana");

            // Para busca de veículos por transportador e tipo
            modelBuilder.Entity<Veiculo>()
                .HasIndex(v => new { v.TransportadorId, v.TipoVeiculo })
                .HasDatabaseName("IX_Veiculos_TransportadorId_TipoVeiculo");

            // Para busca de avaliações por transportador
            modelBuilder.Entity<Avaliacao>()
                .HasIndex(a => a.TransportadorId)
                .HasDatabaseName("IX_Avaliacoes_TransportadorId");

            // Para busca de notificações (ordem decrescente por data)
            modelBuilder.Entity<Notificacao>()
                .HasIndex(n => new { n.UsuarioId, n.DataCriacao })
                .HasDatabaseName("IX_Notificacoes_UsuarioId_DataCriacao");


        }


    }
}