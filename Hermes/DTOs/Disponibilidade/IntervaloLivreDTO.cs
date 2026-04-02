namespace Hermes.DTOs.Disponibilidade
{
    public class IntervaloLivreDTO
    {
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }


        //propriedades apenas para exibicao no front-end
        public string HoraInicio => Inicio.ToString("HH:mm");
        public string HoraFim => Fim.ToString("HH:mm");

    }
}
