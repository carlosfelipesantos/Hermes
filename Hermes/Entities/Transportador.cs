namespace Hermes.Entities
{
    public class Transportador: Usuario
    {
        public string Documento { get; set; }
        public double AvaliacaoMedia { get; set; }
        public ICollection<Frete> FretesAceitos { get; set; } = new List<Frete>();
        public ICollection<Veiculo> Veiculos { get; set; }
    }
}
