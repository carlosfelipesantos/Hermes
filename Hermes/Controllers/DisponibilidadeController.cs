namespace Hermes.Controllers;

using Hermes.DTOs;
using Hermes.DTOs.Disponibilidade;
using Hermes.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize(Roles = "Transportador")]
[ApiController]
[Route("api/[controller]")]
public class DisponibilidadeController : ControllerBase
{
    private readonly IDisponibilidadeService _service;

    public DisponibilidadeController(IDisponibilidadeService service)
    {
        _service = service;
    }

    // Listar minhas janelas
    [HttpGet("meus")]
    public async Task<IActionResult> ListarMinhasJanelas()
    {
        var transportadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var janelas = await _service.ListarJanelasPorTransportador(transportadorId);
        return Ok(janelas);
    }

    // Criar nova janela
    [HttpPost]
    public async Task<IActionResult> CriarJanela(CriarDisponibilidadeBaseDTO dto)
    {
        var transportadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        await _service.CriarJanela(transportadorId, dto);
        return Ok();
    }

    // Atualizar janela
    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarJanela(int id, CriarDisponibilidadeBaseDTO dto)
    {
        var transportadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        await _service.AtualizarJanela(id, dto, transportadorId);
        return NoContent();
    }

    // Deletar janela
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarJanela(int id)
    {
        var transportadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var sucesso = await _service.DeletarJanela(id, transportadorId);
        if (!sucesso) return NotFound();
        return NoContent();
    }

    // Consultar intervalos livres (público)
    [AllowAnonymous]
    [HttpGet("{transportadorId}/intervalos")]
    public async Task<IActionResult> ListarIntervalos(int transportadorId, [FromQuery] DateTime data)
    {
        var intervalos = await _service.ListarIntervalosLivres(transportadorId, data);
        return Ok(intervalos);
    }
}