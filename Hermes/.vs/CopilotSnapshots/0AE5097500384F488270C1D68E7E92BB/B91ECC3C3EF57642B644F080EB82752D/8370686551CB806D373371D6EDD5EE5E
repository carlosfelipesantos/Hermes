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

            //Transportador -> Cliente 1:N (FretesAceitos)
            modelBuilder.Entity<Frete>()
                .HasOne(f => f.Transportador)
                .WithMany(t => t.FretesAceitos)
                .HasForeignKey(f => f.TransportadorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Transportador -> Frete 1:1
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

            // Configurações adicionais para propriedades decimal,valor e nota
            modelBuilder.Entity<Frete>()
              .Property(f => f.Valor)
              .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Avaliacao>()
                .Property(a => a.Nota)
                .HasColumnType("decimal(3,2)");
        }
    }
}