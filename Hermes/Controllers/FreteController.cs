using AutoMapper;
using Hermes.DTOs.Filtro;
using Hermes.DTOs.Frete;
using Hermes.DTOs.Paginacao;
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

        //  LISTAR TODOS COM PAGINAÇÃO (admin ou uso geral)
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

        // LISTAR TODOS (sem paginação)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FreteDTO>>> Listar()
        {
            var fretes = await _freteService.Listar();
            return Ok(_mapper.Map<List<FreteDTO>>(fretes));
        }

        //  LISTAR FRETES DO CLIENTE LOGADO
        [Authorize(Roles = "Cliente")]
        [HttpGet("meus")]
        public async Task<ActionResult<IEnumerable<FreteDTO>>> MeusFretes()
        {
            var clienteId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var fretes = await _freteService.ListarPorCliente(clienteId);
            return Ok(_mapper.Map<List<FreteDTO>>(fretes));
        }

        //  FRETES DISPONÍVEIS COM PAGINAÇÃO (Transportador)
        [Authorize(Roles = "Transportador")]
        [HttpGet("disponiveis")]
        public async Task<ActionResult> FretesDisponiveis([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] TipoVeiculo? tipoVeiculo = null )
        {
            var transportadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var (fretes, total) = await _freteService.ListarDisponiveisPaginado(transportadorId, page, pageSize);
            return Ok(new { Data = fretes, Total = total, Page = page, PageSize = pageSize });
        }


        //  LISTAR FRETES POR CIDADE COM PAGINAÇÃO
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

        // BUSCAR FRETE POR ID
        [AllowAnonymous] // Permite acesso sem autenticação para visualizar detalhes do frete
        [HttpGet("{id}")]
        public async Task<ActionResult<FretePublicoDTO>> Buscar(int id)
        {
            var frete = await _freteService.BuscarPorId(id);
            if (frete == null) return NotFound();
            return Ok(_mapper.Map<FretePublicoDTO>(frete));
        }

        // LISTAR FRETES DO TRANSPORTADOR LOGADO
        [Authorize(Roles = "Transportador")]
        [HttpGet("meus-transportes")]
        public async Task<ActionResult<IEnumerable<FreteDTO>>> MeusTransportes()
        {
            var transportadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var fretes = await _freteService.ListarPorTransportador(transportadorId);
            return Ok(_mapper.Map<List<FreteDTO>>(fretes));
        }

        // CRIAR FRETE (Cliente)
        [Authorize(Roles = "Cliente")]
        [HttpPost]
        public async Task<IActionResult> Criar(CriarFrete dto)
        {
            var clienteId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var frete = _mapper.Map<Entities.Frete>(dto);
            frete.ClienteId = clienteId;

            var freteCriado = await _freteService.Criar(frete);

            return CreatedAtAction(nameof(Buscar), new { id = freteCriado.Id }, _mapper.Map<FreteDTO>(freteCriado));
        }

        // ACEITAR FRETE (Transportador)
        [Authorize(Roles = "Transportador")]
        [HttpPost("{id}/aceitar")]
        public async Task<IActionResult> AceitarFrete(int id)
        {
            var transportadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var sucesso = await _freteService.AceitarFrete(id, transportadorId);

            if (!sucesso) return BadRequest("Frete indisponível ou já aceito");
            return Ok();
        }

        // FINALIZAR FRETE (Transportador dono do frete)
        [Authorize(Roles = "Transportador")]
        [HttpPost("{id}/finalizar")]
        public async Task<IActionResult> FinalizarFrete(int id)
        {
            var transportadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var sucesso = await _freteService.FinalizarFrete(id, transportadorId);

            if (!sucesso) return NotFound();
            return Ok();
        }

        //  ATUALIZAR STATUS
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

        // DELETAR FRETE
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