using Hermes.Enums;

namespace Hermes.Entities
{
    public class Notificacao
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public TipoNotificacao Tipo { get; set; }
        public Boolean Lida { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public int? FreteId { get; set; }
        public Frete? Frete { get; set; }
    }
}
