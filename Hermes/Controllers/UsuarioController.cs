using AutoMapper;
using Hermes.DTOs.Usuario;
using Hermes.Entities;
using Hermes.Enums;
using Hermes.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

    //  CRIAR USUÁRIO (público)
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Criar(CriarUsuario dto)
    {
        if (dto.Tipo == TipoUsuario.Admin)
            return BadRequest("Não é permitido criar admin");


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

    //  USUÁRIO LOGADO
    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<UsuarioDTO>> MeuUsuario()
    {
        var userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier).Value
        );

        var usuario = await _usuarioService.BuscarPorId(userId);

        if (usuario == null)
            return NotFound();

        return Ok(_mapper.Map<UsuarioDTO>(usuario));
    }

    //  DELETAR PRÓPRIA CONTA
    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> Deletar()
    {
        var userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier).Value
        );

        var sucesso = await _usuarioService.Deletar(userId);

        if (!sucesso)
            return NotFound();

        return NoContent();
    }

    //  (OPCIONAL) ADMIN
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioDTO>>> Listar()
    {
        var usuarios = await _usuarioService.Listar();
        return Ok(_mapper.Map<List<UsuarioDTO>>(usuarios));
    }
}