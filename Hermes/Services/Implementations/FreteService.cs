using AutoMapper;
using Hermes.Data;
using Hermes.DTOs.Filtro;
using Hermes.DTOs.Frete;
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
        private readonly INotificacaoService _notificacaoService;
        private readonly IMapper _mapper;
        private readonly IDisponibilidadeService _disponibilidadeService;
   

        public FreteService(HermesBD context, INotificacaoService notificacaoService, IMapper mapper, IDisponibilidadeService disponibilidadeService)
        {
            _context = context;
            _notificacaoService = notificacaoService;
            _mapper = mapper;
            _disponibilidadeService = disponibilidadeService;
        }

      

        public async Task<List<Frete>> ListarConcluidosRecentes(int quantidade)
        {
            return await _context.Fretes
                .AsNoTracking()
                .Where(f => f.Status == StatusFrete.Concluido)
                .OrderByDescending(f => f.DataConclusao)
                .Take(quantidade)
                .ToListAsync();
        }

        public async Task<(List<Frete> data, int total)> ListarDisponiveisFiltrado(
            FreteFiltroDTO filtro, PaginacaoParams paginacao)
        {
            var query = _context.Fretes
                .AsNoTracking()
                .Where(f => f.TransportadorId == null && f.Status == StatusFrete.Pendente)
                .Include(f => f.Cliente)
                .AsQueryable();

            if (filtro.Urgente.HasValue)
                query = query.Where(f => f.Urgente == filtro.Urgente.Value);

            if (!string.IsNullOrEmpty(filtro.Cidade))
                query = query.Where(f => f.CidadeOrigem.Contains(filtro.Cidade));

            if (!string.IsNullOrEmpty(filtro.Bairro))
                query = query.Where(f => f.BairroOrigem.Contains(filtro.Bairro));

            if (!string.IsNullOrEmpty(filtro.Estado))
                query = query.Where(f => f.EstadoOrigem.Contains(filtro.Estado));

            if (!string.IsNullOrEmpty(filtro.DDD))
                query = query.Where(f => f.DDDOrigem.Contains(filtro.DDD));

            if (filtro.Status.HasValue)
                query = query.Where(f => f.Status == filtro.Status.Value);

            if (filtro.ValorMin.HasValue)
                query = query.Where(f => f.Valor >= filtro.ValorMin.Value);

            if (filtro.ValorMax.HasValue)
                query = query.Where(f => f.Valor <= filtro.ValorMax.Value);

            query = query
                .OrderByDescending(f => f.Urgente)
                .ThenByDescending(f => f.DataSolicitacao);

            var total = await query.CountAsync();

            var data = await query
                .Skip((paginacao.Page - 1) * paginacao.PageSize)
                .Take(paginacao.PageSize)
                .ToListAsync();

            return (data, total);
        }

        public async Task<(List<Frete> data, int total)> ListarPaginado(int page, int pageSize)
        {
            var query = _context.Fretes
                .AsNoTracking()
                .Include(f => f.Cliente)
                .Include(f => f.Transportador);

            var total = await query.CountAsync();

            var data = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, total);
        }

        public async Task<(List<FreteDTO> data, int total)> ListarDisponiveisPaginado(int transportadorId, int page, int pageSize, TipoVeiculo? tipoVeiculo=null)
        {
            var transportador = await _context.Transportadores
                .Include(t => t.Veiculos)
                .FirstOrDefaultAsync(t => t.Id == transportadorId);

            var tiposVeiculo = transportador?.Veiculos.Select(v => v.TipoVeiculo).Distinct().ToList() ?? new();

            var query = _context.Fretes
                .AsNoTracking()
                .Where(f => f.TransportadorId == null && f.Status == StatusFrete.Pendente)
                .Include(f => f.Cliente);

            var total = await query.CountAsync();
            var fretes = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var fretesDTO = new List<FreteDTO>();
            foreach (var f in fretes)
            {
                var dto = _mapper.Map<FreteDTO>(f);
                dto.Sugerido = tiposVeiculo.Any(tv => CompatibilidadeCarga.IsCompativel(tv, f.TipoCarga));
                fretesDTO.Add(dto);
            }

            return (fretesDTO, total);
        }

        public async Task<(List<Frete> data, int total)> ListarPorCidadePaginado(string cidade, int page, int pageSize)
        {
            var query = _context.Fretes
                .AsNoTracking()
                .Where(f => f.CidadeOrigem == cidade)
                .Include(f => f.Cliente)
                .Include(f => f.Transportador);

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
                .AsNoTracking()
                .Include(f => f.Cliente)
                .Include(f => f.Transportador)
                .ToListAsync();
        }

        public async Task<Frete> BuscarPorId(int id)
        {
            return await _context.Fretes
                .AsNoTracking()
                .Include(f => f.Cliente)
                .Include(f => f.Transportador)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        
        public async Task<Frete> Criar(Frete frete)
        {
            if (frete.SitioOrigem && string.IsNullOrWhiteSpace(frete.DescricaoOrigem))
                throw new Exception("Descrição da origem é obrigatória para sítio");

            if (frete.SitioDestino && string.IsNullOrWhiteSpace(frete.DescricaoDestino))
                throw new Exception("Descrição do destino é obrigatória para sítio");


            // Validação para frete agendado (quando transportador é informado)
            if (frete.TransportadorId.HasValue && frete.DataHoraInicio != default)
            {
                // 1. Se o transportador não informou o fim previsto, calcular automaticamente
                if (frete.DataHoraFimPrevisto == default)
                {
                    var distancia = CalcularDistancia(
                        frete.LatitudeOrigem, frete.LongitudeOrigem,
                        frete.LatitudeDestino, frete.LongitudeDestino);
                    frete.DuracaoEstimada = CalcularDuracaoEstimada(distancia);
                    frete.DataHoraFimPrevisto = frete.DataHoraInicio + frete.DuracaoEstimada;
                }

                // 2. Verificar se já existe outro frete sobrepondo o intervalo
                var conflito = await _context.Fretes.AnyAsync(f =>
                    f.TransportadorId == frete.TransportadorId &&
                    f.Status != StatusFrete.Cancelado &&
                    f.DataHoraInicio < frete.DataHoraFimPrevisto &&
                    f.DataHoraFimPrevisto > frete.DataHoraInicio);
                if (conflito)
                    throw new Exception("Já existe um frete agendado nesse período.");

                // 3. Verificar se o intervalo está dentro da disponibilidade do transportador
                var intervalosLivres = await _disponibilidadeService.ListarIntervalosLivres(
                    frete.TransportadorId.Value,
                    frete.DataHoraInicio.Date,
                    TimeSpan.FromMinutes(20) // buffer global (pode vir de configuração)
                );

                bool disponivel = intervalosLivres.Any(i =>
                    i.Inicio <= frete.DataHoraInicio && i.Fim >= frete.DataHoraFimPrevisto);

                if (!disponivel)
                    throw new Exception("Horário não disponível para este transportador");
            }

            frete.DataSolicitacao = DateTime.Now;
            frete.Status = StatusFrete.Pendente;

            await _context.Fretes.AddAsync(frete);
            await _context.SaveChangesAsync();

            // Notifica transportadores se for frete imediato (sem transportador definido)
            if (!frete.TransportadorId.HasValue)
            {
                var transportadoresIds = await _context.Transportadores
                    .Select(t => t.Id)
                    .ToListAsync();

                foreach (var id in transportadoresIds)
                {
                    await _notificacaoService.CriarNotificacao(
                        id,
                        "Novo frete disponível",
                        $"Um novo frete #{frete.Id} foi solicitado.",
                        TipoNotificacao.FreteNovo,
                        frete.Id
                    );
                }
            }
            else
            {
                // Notifica apenas o transportador específico (frete agendado)
                await _notificacaoService.CriarNotificacao(
                    frete.TransportadorId.Value,
                    "Nova solicitação de frete",
                    $"Cliente solicitou um frete para {frete.DataHoraInicio:dd/MM/yyyy HH:mm}",
                    TipoNotificacao.FreteNovo,
                    frete.Id
                );
            
        }

            return frete;
        }

        public async Task<IEnumerable<Frete>> BuscarFretesParaTransportador(int transportadorId)
        {
            var transportador = await _context.Transportadores
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == transportadorId);

            if (transportador == null)
                return new List<Frete>();

            if (transportador.Latitude is null || transportador.Longitude is null)
                throw new Exception("Transportador precisa ativar localização.");

            var fretes = await _context.Fretes
                .AsNoTracking()
                .Where(f => f.TransportadorId == null && f.Status == StatusFrete.Pendente)
                .Include(f => f.Cliente)
                .ToListAsync();

            return fretes
                .Where(f =>
                    CalcularDistancia(
                        transportador.Latitude.Value,
                        transportador.Longitude.Value,
                        f.LatitudeOrigem,
                        f.LongitudeOrigem
                    ) <= 20
                )
                .ToList();
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
            const int R = 6371;
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private TimeSpan CalcularDuracaoEstimada(double distanciaKm)
        {
            double velocidadeMedia = 40; // km/h (ajustável)
            double tempoCargaDescarga = 0.5; // 30 minutos
            double horas = distanciaKm / velocidadeMedia + tempoCargaDescarga;
            return TimeSpan.FromHours(horas);
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
                .AsNoTracking()
                .Where(f => f.CidadeOrigem == cidade)
                .ToListAsync();
        }

        public async Task<bool> FinalizarFrete(int id, int transportadorId)
        {
            var frete = await _context.Fretes.FindAsync(id);

            if (frete == null || frete.TransportadorId != transportadorId || frete.Status != StatusFrete.EmTransito)
                return false;

            frete.Status = StatusFrete.Concluido;
            frete.DataConclusao = DateTime.Now;
            frete.DataHoraFimReal = DateTime.Now;

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
                .AsNoTracking()
                .Where(f => f.ClienteId == clienteId)
                .Include(f => f.Cliente)
                .Include(f => f.Transportador)
                .ToListAsync();
        }

        public async Task<IEnumerable<Frete>> ListarPorTransportador(int transportadorId)
        {
            return await _context.Fretes
                .AsNoTracking()
                .Where(f => f.TransportadorId == transportadorId)
                .Include(f => f.Transportador)
                .Include(f => f.Cliente)
                .ToListAsync();
        }

        public async Task<IEnumerable<Frete>> ListarDisponiveis()
        {
            return await _context.Fretes
                .AsNoTracking()
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


        // Confirmar frete agendado (transportador aceita)
        public async Task<bool> ConfirmarFreteAgendado(int freteId, int transportadorId)
        {
            var frete = await _context.Fretes.FindAsync(freteId);

            if (frete == null || frete.TransportadorId != transportadorId)
                return false;

            if (frete.Status != StatusFrete.Pendente)
                return false;

            frete.Status = StatusFrete.Agendado;
            await _context.SaveChangesAsync();

            await _notificacaoService.CriarNotificacao(
                frete.ClienteId,
                "Frete confirmado",
                $"Seu frete foi confirmado pelo transportador",
                TipoNotificacao.FreteAceito,
                frete.Id
            );

            return true;
        }

        // Rejeitar frete agendado (transportador recusa)
        public async Task<bool> RejeitarFreteAgendado(int freteId, int transportadorId)
        {
            var frete = await _context.Fretes.FindAsync(freteId);

            if (frete == null || frete.TransportadorId != transportadorId)
                return false;

            if (frete.Status != StatusFrete.Pendente)
                return false;

            frete.Status = StatusFrete.Cancelado;
            await _context.SaveChangesAsync();

            await _notificacaoService.CriarNotificacao(
                frete.ClienteId,
                "Frete rejeitado",
                $"Infelizmente o transportador não pôde aceitar seu frete",
                TipoNotificacao.FreteCancelado,
                frete.Id
            ); 

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