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
            return await _context.Fretes
                .AsNoTracking()
                .Include(f => f.Cliente)
                .Include(f => f.Transportador)
                .FirstOrDefaultAsync(f => f.Id == id);
        }


        public async Task<Frete> CriarFreteImediato(Frete frete)
        {
            // Validações de sítio
            if (frete.SitioOrigem && string.IsNullOrWhiteSpace(frete.DescricaoOrigem))
                throw new Exception("Descrição da origem é obrigatória para sítio");

            if (frete.SitioDestino && string.IsNullOrWhiteSpace(frete.DescricaoDestino))
                throw new Exception("Descrição do destino é obrigatória para sítio");

            frete.DataSolicitacao = DateTime.Now;
            frete.Status = StatusFrete.Pendente;  // Aguardando aceitação

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
                throw new Exception("Descrição da origem é obrigatória para sítio");

            if (frete.SitioDestino && string.IsNullOrWhiteSpace(frete.DescricaoDestino))
                throw new Exception("Descrição do destino é obrigatória para sítio");

            // VALIDAÇÃO EXPLÍCITA: TransportadorId é obrigatório
            if (!frete.TransportadorId.HasValue)
                throw new Exception("Transportador não informado para frete agendado");

            // VALIDAÇÃO EXPLÍCITA: DataHoraInicio é obrigatória
            if (frete.DataHoraInicio == default)
                throw new Exception("Data e hora de início são obrigatórias para frete agendado");

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
                throw new Exception("Já existe um frete agendado nesse período.");

            // Verificar disponibilidade do transportador (janela de trabalho + buffer)
            var intervalosLivres = await _disponibilidadeService.ListarIntervalosLivres(
                frete.TransportadorId.Value,
                frete.DataHoraInicio.Date,
                TimeSpan.FromMinutes(20) // buffer
            );

            bool disponivel = intervalosLivres.Any(i =>
                i.Inicio <= frete.DataHoraInicio && i.Fim >= frete.DataHoraFimPrevisto);
            if (!disponivel)
                throw new Exception("Horário não disponível para este transportador");

            frete.DataSolicitacao = DateTime.Now;
            frete.Status = StatusFrete.Pendente;  // Aguardando confirmação do transportador

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
            // Tenta calcular com coordenadas se existirem
            if (frete.LatitudeOrigem.HasValue && frete.LongitudeOrigem.HasValue &&
                frete.LatitudeDestino.HasValue && frete.LongitudeDestino.HasValue)
            {
                var distancia = CalcularDistancia(
                    frete.LatitudeOrigem.Value, frete.LongitudeOrigem.Value,
                    frete.LatitudeDestino.Value, frete.LongitudeDestino.Value);
                return CalcularDuracaoEstimada(distancia);
            }

            // Fallback: duração padrão de 1 hora (ajustável)
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
                throw new Exception("Transportador precisa ativar localização.");

            var fretes = await _context.Fretes
                .AsNoTracking()
                .Where(f => f.TransportadorId == null && f.Status == StatusFrete.Pendente)
                .Include(f => f.Cliente)
                .ToListAsync();

            //  Filtrar apenas fretes que possuem coordenadas e estão dentro do raio
            return fretes
                .Where(f => f.LatitudeOrigem.HasValue && f.LongitudeOrigem.HasValue)
                .Where(f =>
                    CalcularDistancia(
                        transportador.Latitude.Value,
                        transportador.Longitude.Value,
                        f.LatitudeOrigem.Value,
                        f.LongitudeOrigem.Value
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


        public async Task<TimeSpan> ObterDuracaoEstimada(int freteId)
        {
            var frete = await BuscarPorId(freteId);
            if (frete == null) throw new Exception("Frete não encontrado");

            return ObterDuracaoEstimadaComFallback(frete);
        }

        private double CalcularDistancia(double? lat1, double? lon1, double? lat2, double? lon2)
        {
            // Se qualquer coordenada for nula, retorna 0 (ou pode lançar exceção)
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
            double velocidadeMedia = 40; // km/h (ajustável)
            double tempoCargaDescarga = 0.5; // 30 minutos
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

            // Verificar se o transportador existe e está ativo
            var transportador = await _context.Transportadores
                .Include(t => t.Veiculos)
                .FirstOrDefaultAsync(t => t.Id == transportadorId && t.Ativo);

            if (transportador == null)
                throw new Exception("Transportador não encontrado ou inativo");

            // Verificar compatibilidade de veículo
            bool veiculoCompativel = transportador.Veiculos
                .Any(v => CompatibilidadeCarga.IsCompativel(v.TipoVeiculo, frete.TipoCarga));

            if (!veiculoCompativel)
                throw new Exception("Você não possui um veículo compatível com este tipo de carga");

            // Definir data/hora de início (ex: 30 minutos a partir de agora)
            var dataHoraInicio = DateTime.Now.AddMinutes(30);

            // Usar o método com fallback para calcular duração estimada (trata coordenadas nulas)
            var duracaoEstimada = ObterDuracaoEstimadaComFallback(frete);
            var dataHoraFimPrevisto = dataHoraInicio + duracaoEstimada;

            // Verificar conflito de agenda
            var intervalosLivres = await _disponibilidadeService.ListarIntervalosLivres(
                transportadorId,
                dataHoraInicio.Date,
                TimeSpan.FromMinutes(20) // buffer
            );

            bool horarioDisponivel = intervalosLivres.Any(i =>
                i.Inicio <= dataHoraInicio && i.Fim >= dataHoraFimPrevisto);

            if (!horarioDisponivel)
                throw new Exception("Não há disponibilidade na agenda para o horário calculado");

            // Atualizar o frete
            frete.TransportadorId = transportadorId;
            frete.Status = StatusFrete.Aceito;
            frete.DataHoraInicio = dataHoraInicio;
            frete.DataHoraFimPrevisto = dataHoraFimPrevisto;
            frete.DuracaoEstimada = duracaoEstimada;

            await _context.SaveChangesAsync();

            // Notificar o cliente
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

        public async Task<bool> AtualizarStatus(int id, StatusFrete novoStatus)
        {
            var frete = await _context.Fretes.FindAsync(id);
            if (frete == null) return false;

            var statusAtual = frete.Status;
            bool transicaoValida = false;

            // ✅ Validação de transições permitidas
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
                    transicaoValida = false; // Nenhuma transição permitida após concluído
                    break;

                case StatusFrete.Cancelado:
                    transicaoValida = false; // Nenhuma transição permitida após cancelado
                    break;

                default:
                    transicaoValida = false;
                    break;
            }

            if (!transicaoValida)
                throw new Exception($"Transição de status inválida: de {statusAtual} para {novoStatus}");

            frete.Status = novoStatus;
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