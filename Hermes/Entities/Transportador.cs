namespace Hermes.Entities
{
    public class Transportador: Usuario
    {
        public string Documento { get; set; }
        public double? AvaliacaoMedia { get; set; }

        public int TotalAvaliacoes  { get; set; }

        public bool Disponivel { get; set; }


        public ICollection<DisponibilidadeBase> Disponibilidades { get; set; } = new List<DisponibilidadeBase>();
        public ICollection<Frete> FretesAceitos { get; set; } = new List<Frete>();
        public ICollection<Veiculo> Veiculos { get; set; } = new List<Veiculo>();   
    }
}
