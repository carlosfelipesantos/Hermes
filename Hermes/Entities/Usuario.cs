namespace Hermes.Entities
{
    public class Usuario
    {  
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Tipo { get; set; }

        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public string FotoPerfil { get; set; }
        public DateTime Data_Cadastro { get; set; }
        public string DDD { get; set; }
        public Boolean Ativo { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }

        public ICollection<Notificacao> Notificacoes { get; set; }
    }
}
