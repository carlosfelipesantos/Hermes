using AutoMapper;
using Hermes.DTOs.Usuario;
using Hermes.Entities;
using Hermes.Enums;
using Hermes.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{   
    private readonly IUsuarioService _usuarioService;
    private readonly IMapper _mapper;

    public UsuarioController(IMapper mapper, IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioDTO>>> Listar()
    {
        var usuarios = await _usuarioService.Listar();
        return Ok(_mapper.Map<List<UsuarioDTO>>(usuarios));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UsuarioDTO>> Buscar(int id)
    {
        var usuario = await _usuarioService.BuscarPorId(id);

        if (usuario == null)
            return NotFound();

        return Ok(_mapper.Map<UsuarioDTO>(usuario));
    }

    [HttpPost]
    public async Task<IActionResult> Criar(CriarUsuario dto)
    {
        if (dto.Tipo == TipoUsuario.Cliente)
        {
            var cliente = _mapper.Map<Cliente>(dto);
            cliente.DataCadastro = DateTime.Now;
            cliente.Ativo = true;

            await _usuarioService.Criar(cliente);
            return Ok(_mapper.Map<UsuarioDTO>(cliente));
        }
        else if (dto.Tipo == TipoUsuario.Transportador)
        {
            var transportador = _mapper.Map<Transportador>(dto);
            transportador.DataCadastro = DateTime.Now;
            transportador.Ativo = true;

            await _usuarioService.Criar(transportador);
            return Ok(_mapper.Map<UsuarioDTO>(transportador));
        }

        return BadRequest("Tipo inválido");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Deletar(int id)
    {
        var sucesso = await _usuarioService.Deletar(id);

        if (!sucesso)
            return NotFound();

        return NoContent();
    }
}
