using AutoMapper;
using Hermes.DTOs.Filtro;
using Hermes.DTOs.Frete;
using Hermes.DTOs.Paginacao;
using Hermes.Entities;
using Hermes.Enums;
using Hermes.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hermes.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FreteController : ControllerBase
    {
        private readonly IFreteService _freteService;
        private readonly IMapper _mapper;

        public FreteController(IFreteService freteService, IMapper mapper)
        {
            _freteService = freteService;
            _mapper = mapper;
        }

        // GET /api/Frete/{id}/duracao-estimada
        [Authorize(Roles = "Transportador")]
        [HttpGet("{id}/duracao-estimada")]
        public async Task<IActionResult> ObterDuracaoEstimada(int id)
        {
            var frete = await _freteService.BuscarPorId(id);
            if (frete == null) return NotFound();

           var duracao = await _freteService.ObterDuracaoEstimada(id);

            return Ok(new { duracaoEstimada = duracao });
        }

        [AllowAnonymous]
        [HttpGet("concluidos/home")]
        public async Task<ActionResult<IEnumerable<FretePublicoDTO>>> FretesConcluidosHome()
        {
            var fretes = await _freteService.ListarConcluidosRecentes(10); // pega 10 últimos
            return Ok(_mapper.Map<List<FretePublicoDTO>>(fretes));
        }


        [AllowAnonymous]
        [HttpGet("disponiveis/filtradopaginado")]
        public async Task<IActionResult> GetDisponiveis(
            [FromQuery] FreteFiltroDTO filtro,
            [FromQuery] PaginacaoParams paginacao)
        {
            var (fretes, total) = await _freteService
                .ListarDisponiveisFiltrado(filtro, paginacao);
            var fretesDTO = _mapper.Map<List<FretePublicoDTO>>(fretes);

            return Ok(new
            {
                Data = fretesDTO,
                Total = total,
                Page = paginacao.Page,
                PageSize = paginacao.PageSize
            });
        }

        // Listar todos com paginacao
        [AllowAnonymous]
        [HttpGet("paginado")]
        public async Task<ActionResult> ListarPaginado([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var (fretes, total) = await _freteService.ListarPaginado(page, pageSize);
            var fretesDTO = _mapper.Map<List<FretePublicoDTO>>(fretes);

            return Ok(new
            {
                Data = fretesDTO,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }

        // Listar todos sem paginacao
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FreteDTO>>> Listar()
        {
            var fretes = await _freteService.Listar();
            return Ok(_mapper.Map<List<FreteDTO>>(fretes));
        }

        // Listar fretes do cliente logado
        [Authorize(Roles = "Cliente")]
        [HttpGet("meus")]
        public async Task<ActionResult<IEnumerable<FreteDTO>>> MeusFretes()
        {
            var clienteId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var fretes = await _freteService.ListarPorCliente(clienteId);
            return Ok(_mapper.Map<List<FreteDTO>>(fretes));
        }

        //  Listar fretes disponiveis para transportador
        [Authorize(Roles = "Transportador")]
        [HttpGet("disponiveis")]
        public async Task<ActionResult> FretesDisponiveis([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] TipoVeiculo? tipoVeiculo = null )
        {
            var transportadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var (fretes, total) = await _freteService.ListarDisponiveisPaginado(transportadorId, page, pageSize);
            return Ok(new { Data = fretes, Total = total, Page = page, PageSize = pageSize });
        }


        //  Listar fretes por cidade com paginacao
        [AllowAnonymous]
        [HttpGet("cidade/{cidade}")]
        public async Task<ActionResult> FretesPorCidade(string cidade, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var (fretes, total) = await _freteService.ListarPorCidadePaginado(cidade, page, pageSize);

            var fretesDTO = _mapper.Map<List<FretePublicoDTO>>(fretes);
            return Ok(new
            {
                Data = fretesDTO,
                Total = total,
                Page = page,
                PageSize = pageSize
            });
        }

        // Buscar Frete por ID
        [AllowAnonymous] 
        [HttpGet("{id}")]
        public async Task<ActionResult<FretePublicoDTO>> Buscar(int id)
        {
            var frete = await _freteService.BuscarPorId(id);
            if (frete == null) return NotFound();
            return Ok(_mapper.Map<FretePublicoDTO>(frete));
        }

        // Listar Fretes do transportador logado
        [Authorize(Roles = "Transportador")]
        [HttpGet("meus-transportes")]
        public async Task<ActionResult<IEnumerable<FreteDTO>>> MeusTransportes()
        {
            var transportadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var fretes = await _freteService.ListarPorTransportador(transportadorId);
            return Ok(_mapper.Map<List<FreteDTO>>(fretes));
        }

        [Authorize(Roles = "Cliente")]
        [HttpPost("imediato")]
        public async Task<IActionResult> CriarFreteImediato([FromBody] CriarFrete dto)
        {
            var clienteId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var frete = _mapper.Map<Frete>(dto);
            frete.ClienteId = clienteId;

            var freteCriado = await _freteService.CriarFreteImediato(frete);
            return CreatedAtAction(nameof(Buscar), new { id = freteCriado.Id }, _mapper.Map<FreteDTO>(freteCriado));
        }

        [Authorize(Roles = "Cliente")]
        [HttpPost("agendado")]
        public async Task<IActionResult> CriarFreteAgendado([FromBody] CriarFreteAgendadoDTO dto)
        {
            var clienteId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var frete = _mapper.Map<Frete>(dto);
            frete.ClienteId = clienteId;

            var freteCriado = await _freteService.CriarFreteAgendado(frete);
            return CreatedAtAction(nameof(Buscar), new { id = freteCriado.Id }, _mapper.Map<FreteDTO>(freteCriado));
        }

        // Aceitar Frete (Transportador)
        [Authorize(Roles = "Transportador")]
        [HttpPost("{id}/aceitar")]
        public async Task<IActionResult> AceitarFrete(int id)
        {
            var transportadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var sucesso = await _freteService.AceitarFrete(id, transportadorId);

            if (!sucesso) return BadRequest("Frete indisponível ou já aceito");
            return Ok();
        }

        // Finalizar Frete (Transportador dono do frete)
        [Authorize(Roles = "Transportador")]
        [HttpPost("{id}/finalizar")]
        public async Task<IActionResult> FinalizarFrete(int id)
        {
            var transportadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var sucesso = await _freteService.FinalizarFrete(id, transportadorId);

            if (!sucesso) return NotFound();
            return Ok();
        }

        //  Atualizar Status do Frete
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, AtualizarStatusFrete dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            var frete = await _freteService.BuscarPorId(id);
            if (frete == null) return NotFound();

            if (role != "Admin")
            {
                if (role == "Cliente" && frete.ClienteId != userId)
                    return Forbid();

                if (role == "Transportador" && frete.TransportadorId != userId)
                    return Forbid();
            }
            var sucesso = await _freteService.AtualizarStatus(id, dto.Status);
            if (!sucesso) return NotFound();
            return Ok();
        }


        // Confirmar Frete Agendado (Transportador)
        [Authorize(Roles = "Transportador")]
        [HttpPost("{id}/confirmar")]
        public async Task<IActionResult> ConfirmarFreteAgendado(int id)
        {
            var transportadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var sucesso = await _freteService.ConfirmarFreteAgendado(id, transportadorId);

            if (!sucesso) return BadRequest("Não foi possível confirmar o frete");
            return Ok();
        }

        // Rejeitar Frete Agendado (Transportador)
        [Authorize(Roles = "Transportador")]
        [HttpPost("{id}/rejeitar")]
        public async Task<IActionResult> RejeitarFreteAgendado(int id)
        {
            var transportadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var sucesso = await _freteService.RejeitarFreteAgendado(id, transportadorId);

            if (!sucesso) return BadRequest("Não foi possível rejeitar o frete");
            return Ok();
        }


        // Deletar Frete
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            var frete = await _freteService.BuscarPorId(id);
            if (frete == null) return NotFound();

            if (role != "Admin")
            {
                if (role == "Cliente" && frete.ClienteId != userId)
                    return Forbid();

                if (role == "Transportador" && frete.TransportadorId != userId)
                    return Forbid();
            }
            var sucesso = await _freteService.Deletar(id);
            if (!sucesso) return NotFound();
            return NoContent();
        }
    }
}