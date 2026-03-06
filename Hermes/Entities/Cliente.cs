namespace Hermes.Entities
{
    public class Cliente: Usuario
    {
        public DateTime DataNasc { get; set; }
        public ICollection<Frete> FretesSolicitados { get; set; } = new List<Frete>();

    }
}
