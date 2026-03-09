namespace Hermes.DTOs.Avaliacao
{
    public class AvaliacaoDTO
    {
        public int Id { get; set; }
        public double Nota { get; set; }
        public string Comentario { get; set; }
        public DateTime DataAvaliacao { get; set; }

        public int FreteId { get; set; }
    }
}
