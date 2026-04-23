using System.ComponentModel.DataAnnotations;

namespace Hermes.DTOs.Paginacao
{
    public class PaginacaoParams
    {
        private const int MAX_PAGE_SIZE = 100; // Limite máximo de registros por página

        private int _pageSize = 10;
        private int _page = 1;

        public int Page
        {
            get => _page;
            set => _page = value < 1 ? 1 : value;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value < 1 ? MAX_PAGE_SIZE : (value > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : value);
        }
    }
}