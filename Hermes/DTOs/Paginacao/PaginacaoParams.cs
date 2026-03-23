namespace Hermes.DTOs.Paginacao
{
    public class PaginacaoParams
    {
        public int Page { get; set; } = 1; //caso nao seja informado valor, o valor padrao sera 1
        public int PageSize { get; set; } = 10;
    }
}
