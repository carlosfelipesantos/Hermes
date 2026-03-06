namespace Hermes.Entities
{
    public class Notificacao
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public string Tipo { get; set; }
        public Boolean Lida { get; set; }
        public DateTime DataCriacao { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public int? FreteId { get; set; }
        public Frete Frete { get; set; }
    }
}
