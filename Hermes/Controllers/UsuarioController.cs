using AutoMapper;
using Hermes.DTOs.Usuario;
using Hermes.Entities;
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
        var usuario = _mapper.Map<Usuario>(dto);

        usuario.DataCadastro = DateTime.Now;
        usuario.Ativo = true;

        await _usuarioService.Criar(usuario);

        return Ok(_mapper.Map<UsuarioDTO>(usuario));
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
