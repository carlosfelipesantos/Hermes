using Hermes.Data;
using Hermes.DTOs.Filtro;
using Hermes.DTOs.Paginacao;
using Hermes.Entities;
using Hermes.Enums;
using Hermes.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Services.Implementations
{
    public class FreteService : IFreteService
    {
        private readonly HermesBD _context;
        private readonly NotificacaoService _notificacaoService;

        public FreteService(HermesBD context, NotificacaoService notificacaoService)
        {
            _context = context;
            _notificacaoService = notificacaoService;
        }

        //filtrados
        public async Task<(List<Frete> data, int total)> ListarDisponiveisFiltrado(
    FreteFiltroDTO filtro, PaginacaoParams paginacao)
        {
            var query = _context.Fretes
                .Where(f => f.TransportadorId == null && f.Status == StatusFrete.Pendente)
                .Include(f => f.Cliente)
                .AsQueryable();

            // FILTROS

            if (filtro.Urgente.HasValue)
                query = query.Where(f => f.Urgente == filtro.Urgente.Value);

            if (!string.IsNullOrEmpty(filtro.Cidade))
                query = query.Where(f => f.CidadeOrigem.Contains(filtro.Cidade));

            if (filtro.Status.HasValue)
                query = query.Where(f => f.Status == filtro.Status.Value);

            if (filtro.ValorMin.HasValue)
                query = query.Where(f => f.Valor >= filtro.ValorMin.Value);

            if (filtro.ValorMax.HasValue)
                query = query.Where(f => f.Valor <= filtro.ValorMax.Value);

            // ORDENAÇÃO 
            query = query
                .OrderByDescending(f => f.Urgente)
                .ThenByDescending(f => f.DataSolicitacao);

            // TOTAL
            var total = await query.CountAsync();

            // PAGINAÇÃO
            var data = await query
                .Skip((paginacao.Page - 1) * paginacao.PageSize)
                .Take(paginacao.PageSize)
                .ToListAsync();

            return (data, total);
        }


        public async Task<(List<Frete> data, int total)> ListarPaginado(int page, int pageSize)
        { 
            var query = _context.Fretes 
                .Include(f => f.Cliente) //diz que ao buscar os fretes, ele deve incluir os dados do cliente relacionado
                .Include(f => f.Transportador) 
                .AsQueryable();

                var total = await query.CountAsync();

            var data = await query
                .Skip((page - 1) * pageSize) //significa pular os primeiros registros, se for a página 2, ele pula os primeiros 10 registros (1-10) e mostra os próximos 10 (11-20)
                .Take(pageSize)
                .ToListAsync();

            return(data, total);
        }

        // Paginação para fretes disponíveis (Transportador)
        public async Task<(List<Frete> data, int total)> ListarDisponiveisPaginado(int transportadorId, int page, int pageSize)
        {
            var query = _context.Fretes
                .Where(f => f.TransportadorId == null && f.Status == StatusFrete.Pendente)
                .Include(f => f.Cliente)
                .AsQueryable();

            var total = await query.CountAsync();

            var data = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, total);
        }

        // Paginação para fretes por cidade
        public async Task<(List<Frete> data, int total)> ListarPorCidadePaginado(string cidade, int page, int pageSize)
        {
            var query = _context.Fretes
                .Where(f => f.CidadeOrigem.ToLower() == cidade.ToLower())
                .Include(f => f.Cliente)
                .Include(f => f.Transportador)
                .AsQueryable();

            var total = await query.CountAsync();

            var data = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, total);
        }

        public async Task<IEnumerable<Frete>> Listar()
        {
            return await _context.Fretes
                 .Include(f => f.Cliente)
                 .Include(f => f.Transportador)
                 .ToListAsync();
        }

        public async Task<Frete> BuscarPorId(int id)
        {
            return await _context.Fretes
                .Include(f => f.Cliente)
                .Include(f => f.Transportador)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Frete> Criar(Frete frete)
        {
            // VALIDAÇÕES DE SITIO
            if (frete.SitioOrigem && string.IsNullOrWhiteSpace(frete.DescricaoOrigem))
                throw new Exception("Descrição da origem é obrigatória para sítio");

            if (frete.SitioDestino && string.IsNullOrWhiteSpace(frete.DescricaoDestino))
                throw new Exception("Descrição do destino é obrigatória para sítio");

            // VALIDAÇÃO DE HORÁRIO (se transportador e horário definidos)
            if (frete.TransportadorId.HasValue && frete.DataAgendada.HasValue && frete.HoraAgendada.HasValue)
            {
                bool existe = await _context.Fretes.AnyAsync(f =>
                    f.TransportadorId == frete.TransportadorId &&
                    f.DataAgendada == frete.DataAgendada &&
                    f.HoraAgendada == frete.HoraAgendada
                );

                if (existe)
                    throw new Exception("Horário já ocupado para este transportador");
            }

            frete.DataSolicitacao = DateTime.Now;
            frete.Status = StatusFrete.Pendente;

            _context.Fretes.Add(frete);
            await _context.SaveChangesAsync();

            // Notificação para todos os transportadores (somente fretes abertos)
            if (!frete.TransportadorId.HasValue)
            {
                var transportadores = await _context.Transportadores.ToListAsync();
                foreach (var t in transportadores)
                {
                    await _notificacaoService.CriarNotificacao(
                        t.Id,
                        "Novo frete disponível",
                        $"Um novo frete #{frete.Id} foi solicitado.",
                        TipoNotificacao.FreteNovo,
                        frete.Id
                    );
                }
            }

            return frete;
        }

        public async Task<IEnumerable<Frete>> BuscarFretesParaTransportador(int transportadorId)
        {
            var transportador = await _context.Transportadores
                .FirstOrDefaultAsync(t => t.Id == transportadorId);

            if (transportador == null)
                return new List<Frete>();

            var fretes = await _context.Fretes
                .Where(f => f.TransportadorId == null && f.Status == StatusFrete.Pendente)
                .Include(f => f.Cliente)
                .ToListAsync();

            if (transportador.Latitude is null || transportador.Longitude is null)
                throw new Exception("Transportador precisa ativar localização.");

            var fretesProximos = fretes
                .Where(f =>
                    CalcularDistancia(
                        transportador.Latitude.Value,
                        transportador.Longitude.Value,
                        f.LatitudeOrigem,
                        f.LongitudeOrigem
                    ) <= 20
                );

            return fretesProximos;
        }

        public string GerarMensagemWhatsApp(Frete frete)
        {
            var origem = frete.SitioOrigem
                ? $"Sítio - {frete.DescricaoOrigem}"
                : $"{frete.CidadeOrigem} - {frete.BairroOrigem}";

            var destino = frete.SitioDestino
                ? $"Sítio - {frete.DescricaoDestino}"
                : $"{frete.CidadeDestino} - {frete.BairroDestino}";

            return $"Olá, vi seu frete no Hermes.%0A" +
                   $"Origem: {origem}%0A" +
                   $"Destino: {destino}%0A" +
                   $"Carga: {frete.DescricaoCarga}%0A" +
                   $"Valor: R$ {frete.Valor}";
        }

        private double CalcularDistancia(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371;

            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;

            var a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1 * Math.PI / 180) *
                Math.Cos(lat2 * Math.PI / 180) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
        }

        public async Task<bool> AceitarFrete(int freteId, int transportadorId)
        {
            var frete = await _context.Fretes.FindAsync(freteId);

            if (frete == null || frete.TransportadorId != null || frete.Status != StatusFrete.Pendente)
                return false;

            frete.TransportadorId = transportadorId;
            frete.Status = StatusFrete.Aceito;

            await _context.SaveChangesAsync();

            await _notificacaoService.CriarNotificacao(
                frete.ClienteId,
                "Frete aceito",
                $"Seu frete #{frete.Id} foi aceito pelo transportador {frete.TransportadorId}.",
                TipoNotificacao.FreteAceito,
                frete.Id
            );

            return true;
        }

        public async Task<IEnumerable<Frete>> ListarPorCidade(string cidade)
        {
            return await _context.Fretes
                .Where(f => f.CidadeOrigem.ToLower() == cidade.ToLower())
                .ToListAsync();
        }

        public async Task<bool> FinalizarFrete(int id, int transportadorId)
        {
            var frete = await _context.Fretes.FindAsync(id);

            if (frete == null || frete.TransportadorId != transportadorId || frete.Status != StatusFrete.EmTransito)
                return false;

            frete.Status = StatusFrete.Concluido;
            frete.DataConclusao = DateTime.Now;

            await _context.SaveChangesAsync();

            await _notificacaoService.CriarNotificacao(
                frete.ClienteId,
                "Frete concluído",
                $"O frete #{frete.Id} foi concluído.",
                TipoNotificacao.FreteFinalizado,
                frete.Id
            );

            return true;
        }

        public async Task<IEnumerable<Frete>> ListarPorCliente(int clienteId)
        {
            return await _context.Fretes
                .Where(f => f.ClienteId == clienteId)
                .Include(f => f.Cliente)
                .Include(f => f.Transportador)
                .ToListAsync();
        }

        public async Task<IEnumerable<Frete>> ListarPorTransportador(int transportadorId)
        {
            return await _context.Fretes
                .Where(f => f.TransportadorId == transportadorId)
                .Include(f => f.Transportador)
                .Include(f => f.Cliente)
                .ToListAsync();
        }

        public async Task<IEnumerable<Frete>> ListarDisponiveis()
        {
            return await _context.Fretes
                .Where(f => f.TransportadorId == null && f.Status == StatusFrete.Pendente)
                .Include(f => f.Cliente)
                .ToListAsync();
        }

        public async Task<bool> AtualizarStatus(int id, StatusFrete status)
        {
            var frete = await _context.Fretes.FindAsync(id);
            if (frete == null) return false;

            frete.Status = status;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Deletar(int id)
        {
            var frete = await _context.Fretes.FindAsync(id);
            if (frete == null) return false;

            _context.Fretes.Remove(frete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}