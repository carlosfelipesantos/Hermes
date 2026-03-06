namespace Hermes.Entities
{
    public class Avaliacao
    {
        public int Id { get; set; }
        public double Nota { get; set; }
        public string Comentario { get; set; }
        public DateTime DataAvaliacao { get; set; }

        public int FreteId { get; set; }
        public Frete Frete { get; set; }
    }
}
