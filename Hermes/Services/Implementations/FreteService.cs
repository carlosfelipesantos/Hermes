using AutoMapper;
using Hermes.Data;
using Hermes.DTOs.Filtro;
using Hermes.DTOs.Frete;
using Hermes.DTOs.Paginacao;
using Hermes.Entities;
using Hermes.Enums;
using Hermes.Exceptions;
using Hermes.Services.Interfaces;
using Hermes.Utils;
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

        public async Task<(List<FreteDTO> data, int total)> ListarDisponiveisPaginado(
            int transportadorId,
            int page,
            int pageSize,
            TipoVeiculo? tipoVeiculo = null)
        {
            var transportador = await _context.Transportadores
                .Include(t => t.Veiculos)
                .FirstOrDefaultAsync(t => t.Id == transportadorId);

            var tiposVeiculo = transportador?.Veiculos.Select(v => v.TipoVeiculo).Distinct().ToList() ?? new();

            // Busca TODOS os fretes disponíveis (sem paginação ainda)
            var todosFretes = await _context.Fretes
                .AsNoTracking()
                .Where(f => f.TransportadorId == null && f.Status == StatusFrete.Pendente)
                .Include(f => f.Cliente)
                .ToListAsync();

            // Aplica filtro em memória (se necessário)
            var fretesFiltrados = todosFretes.AsEnumerable();

            if (tipoVeiculo.HasValue)
            {
                fretesFiltrados = fretesFiltrados.Where(f =>
                    CompatibilidadeCarga.IsCompativel(tipoVeiculo.Value, f.TipoCarga));
            }

            var fretesList = fretesFiltrados.ToList();
            var total = fretesList.Count();

            // Aplica paginação em memória
            var fretes = fretesList
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

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
            var frete = await _context.Fretes
                .AsNoTracking()
                .Include(f => f.Cliente)
                .Include(f => f.Transportador)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (frete == null)
                throw new NotFoundException($"Frete {id} não encontrado");

            return frete;
        }

        public async Task<Frete> CriarFreteImediato(Frete frete)
        {
            // Validações de sítio
            if (frete.SitioOrigem && string.IsNullOrWhiteSpace(frete.DescricaoOrigem))
                throw new BusinessException("Descrição da origem é obrigatória para sítio");

            if (frete.SitioDestino && string.IsNullOrWhiteSpace(frete.DescricaoDestino))
                throw new BusinessException("Descrição do destino é obrigatória para sítio");

            frete.DataSolicitacao = TimeHelper.Now;
            frete.Status = StatusFrete.Pendente;

            _context.Fretes.Add(frete);
            await _context.SaveChangesAsync();

            // Notificar todos os transportadores ativos
            var transportadoresIds = await _context.Transportadores
                .Where(t => t.Ativo)
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

            return frete;
        }

        public async Task<Frete> CriarFreteAgendado(Frete frete)
        {
            // Validações de sítio
            if (frete.SitioOrigem && string.IsNullOrWhiteSpace(frete.DescricaoOrigem))
                throw new BusinessException("Descrição da origem é obrigatória para sítio");

            if (frete.SitioDestino && string.IsNullOrWhiteSpace(frete.DescricaoDestino))
                throw new BusinessException("Descrição do destino é obrigatória para sítio");

            if (!frete.TransportadorId.HasValue)
                throw new BusinessException("Transportador não informado para frete agendado");

            if (frete.DataHoraInicio == default)
                throw new BusinessException("Data e hora de início são obrigatórias para frete agendado");

            // Calcular duração estimada se não informada
            if (frete.DataHoraFimPrevisto == default)
            {
                frete.DuracaoEstimada = ObterDuracaoEstimadaComFallback(frete);
                frete.DataHoraFimPrevisto = frete.DataHoraInicio + frete.DuracaoEstimada;
            }

            // Verificar conflito com outro frete no mesmo período
            var conflito = await _context.Fretes.AnyAsync(f =>
                f.TransportadorId == frete.TransportadorId.Value &&
                f.Status != StatusFrete.Cancelado &&
                f.DataHoraInicio < frete.DataHoraFimPrevisto &&
                f.DataHoraFimPrevisto > frete.DataHoraInicio);

            if (conflito)
                throw new ConflictException("Já existe um frete agendado nesse período.");

            // Verificar disponibilidade do transportador (janela de trabalho + buffer)
            var intervalosLivres = await _disponibilidadeService.ListarIntervalosLivres(
                frete.TransportadorId.Value,
                frete.DataHoraInicio.Date,
                TimeSpan.FromMinutes(20));

            bool disponivel = intervalosLivres.Any(i =>
                i.Inicio <= frete.DataHoraInicio && i.Fim >= frete.DataHoraFimPrevisto);

            if (!disponivel)
                throw new ConflictException("Horário não disponível para este transportador");

            frete.DataSolicitacao = TimeHelper.Now;
            frete.Status = StatusFrete.Pendente;

            _context.Fretes.Add(frete);
            await _context.SaveChangesAsync();

            // Notificar apenas o transportador escolhido
            await _notificacaoService.CriarNotificacao(
                frete.TransportadorId.Value,
                "Nova solicitação de frete",
                $"Cliente solicitou um frete para {frete.DataHoraInicio:dd/MM/yyyy HH:mm}",
                TipoNotificacao.FreteNovo,
                frete.Id
            );

            return frete;
        }

        private TimeSpan ObterDuracaoEstimadaComFallback(Frete frete)
        {
            if (frete.LatitudeOrigem.HasValue && frete.LongitudeOrigem.HasValue &&
                frete.LatitudeDestino.HasValue && frete.LongitudeDestino.HasValue)
            {
                var distancia = CalcularDistancia(
                    frete.LatitudeOrigem.Value, frete.LongitudeOrigem.Value,
                    frete.LatitudeDestino.Value, frete.LongitudeDestino.Value);
                return CalcularDuracaoEstimada(distancia);
            }

            return TimeSpan.FromHours(1);
        }

        public async Task<IEnumerable<Frete>> BuscarFretesParaTransportador(int transportadorId)
        {
            var transportador = await _context.Transportadores
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == transportadorId);

            if (transportador == null)
                return new List<Frete>();

            if (transportador.Latitude is null || transportador.Longitude is null)
                throw new BusinessException("Transportador precisa ativar localização.");

            var fretes = await _context.Fretes
                .AsNoTracking()
                .Where(f => f.TransportadorId == null && f.Status == StatusFrete.Pendente)
                .Include(f => f.Cliente)
                .ToListAsync();

            return fretes
                .Where(f => f.LatitudeOrigem.HasValue && f.LongitudeOrigem.HasValue)
                .Where(f =>
                    CalcularDistancia(
                        transportador.Latitude.Value,
                        transportador.Longitude.Value,
                        f.LatitudeOrigem.Value,
                        f.LongitudeOrigem.Value) <= 20)
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

        public async Task<TimeSpan> ObterDuracaoEstimada(int freteId)
        {
            var frete = await BuscarPorId(freteId);
            if (frete == null)
                throw new NotFoundException($"Frete {freteId} não encontrado");

            return ObterDuracaoEstimadaComFallback(frete);
        }

        private double CalcularDistancia(double? lat1, double? lon1, double? lat2, double? lon2)
        {
            if (!lat1.HasValue || !lon1.HasValue || !lat2.HasValue || !lon2.HasValue)
                return 0;

            const int R = 6371;
            var dLat = (lat2.Value - lat1.Value) * Math.PI / 180;
            var dLon = (lon2.Value - lon1.Value) * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1.Value * Math.PI / 180) * Math.Cos(lat2.Value * Math.PI / 180) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private TimeSpan CalcularDuracaoEstimada(double distanciaKm)
        {
            double velocidadeMedia = 40;
            double tempoCargaDescarga = 0.5;
            double horas = distanciaKm / velocidadeMedia + tempoCargaDescarga;
            return TimeSpan.FromHours(horas);
        }

        public async Task<bool> AceitarFrete(int freteId, int transportadorId)
        {
            var frete = await _context.Fretes
                .Include(f => f.Cliente)
                .FirstOrDefaultAsync(f => f.Id == freteId);

            if (frete == null || frete.TransportadorId != null || frete.Status != StatusFrete.Pendente)
                return false;

            var transportador = await _context.Transportadores
                .Include(t => t.Veiculos)
                .FirstOrDefaultAsync(t => t.Id == transportadorId && t.Ativo);

            if (transportador == null)
                throw new NotFoundException("Transportador não encontrado ou inativo");

            bool veiculoCompativel = transportador.Veiculos
                .Any(v => CompatibilidadeCarga.IsCompativel(v.TipoVeiculo, frete.TipoCarga));

            if (!veiculoCompativel)
                throw new BusinessException("Você não possui um veículo compatível com este tipo de carga");

            var dataHoraInicio = TimeHelper.Now.AddMinutes(30);
            var duracaoEstimada = ObterDuracaoEstimadaComFallback(frete);
            var dataHoraFimPrevisto = dataHoraInicio + duracaoEstimada;

            var intervalosLivres = await _disponibilidadeService.ListarIntervalosLivres(
                transportadorId,
                dataHoraInicio.Date,
                TimeSpan.FromMinutes(20));

            bool horarioDisponivel = intervalosLivres.Any(i =>
                i.Inicio <= dataHoraInicio && i.Fim >= dataHoraFimPrevisto);

            if (!horarioDisponivel)
                throw new ConflictException("Não há disponibilidade na agenda para o horário calculado");

            frete.TransportadorId = transportadorId;
            frete.Status = StatusFrete.Aceito;
            frete.DataHoraInicio = dataHoraInicio;
            frete.DataHoraFimPrevisto = dataHoraFimPrevisto;
            frete.DuracaoEstimada = duracaoEstimada;

            await _context.SaveChangesAsync();

            await _notificacaoService.CriarNotificacao(
                frete.ClienteId,
                "Frete aceito",
                $"Seu frete #{frete.Id} foi aceito por {transportador.Nome}. Início previsto: {dataHoraInicio:dd/MM/yyyy HH:mm}",
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
            frete.DataConclusao = TimeHelper.Now;
            frete.DataHoraFimReal = TimeHelper.Now;

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

        public async Task<bool> AtualizarStatus(int id, StatusFrete novoStatus)
        {
            var frete = await _context.Fretes.FindAsync(id);
            if (frete == null)
                throw new NotFoundException($"Frete {id} não encontrado");

            var statusAtual = frete.Status;
            bool transicaoValida = false;

            switch (statusAtual)
            {
                case StatusFrete.Pendente:
                    transicaoValida = novoStatus == StatusFrete.Aceito ||
                                      novoStatus == StatusFrete.Agendado ||
                                      novoStatus == StatusFrete.Cancelado;
                    break;
                case StatusFrete.Aceito:
                    transicaoValida = novoStatus == StatusFrete.EmTransito ||
                                      novoStatus == StatusFrete.Cancelado;
                    break;
                case StatusFrete.Agendado:
                    transicaoValida = novoStatus == StatusFrete.EmTransito ||
                                      novoStatus == StatusFrete.Cancelado;
                    break;
                case StatusFrete.EmTransito:
                    transicaoValida = novoStatus == StatusFrete.Concluido;
                    break;
                case StatusFrete.Concluido:
                case StatusFrete.Cancelado:
                    transicaoValida = false;
                    break;
                default:
                    transicaoValida = false;
                    break;
            }

            if (!transicaoValida)
                throw new BusinessException($"Transição de status inválida: de {statusAtual} para {novoStatus}");

            frete.Status = novoStatus;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ConfirmarFreteAgendado(int freteId, int transportadorId)
        {
            var frete = await _context.Fretes.FindAsync(freteId);
            if (frete == null || frete.TransportadorId != transportadorId)
                return false;

            if (frete.Status != StatusFrete.Pendente)
                return false;

            var intervalosLivres = await _disponibilidadeService.ListarIntervalosLivres(
                transportadorId,
                frete.DataHoraInicio.Date);
            bool disponivel = intervalosLivres.Any(i =>
                i.Inicio <= frete.DataHoraInicio && i.Fim >= frete.DataHoraFimPrevisto);

            if (!disponivel)
                throw new ConflictException("O horário não está mais disponível para este frete.");

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
            if (frete == null)
                throw new NotFoundException($"Frete {id} não encontrado");

            if (frete.Status == StatusFrete.EmTransito ||
                frete.Status == StatusFrete.Aceito ||
                frete.Status == StatusFrete.Agendado ||
                frete.Status == StatusFrete.Concluido)
            {
                throw new BusinessException($"Não é possível deletar um frete com status {frete.Status}");
            }

            _context.Fretes.Remove(frete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}