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

    [Authorize(Roles = "Transportador")]
    [HttpGet("meus")]
    public async Task<IActionResult> ListarMinhasDisponibilidades()
    {
        var transportadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var disponibilidades = await _service.ListarDisponibilidadesPorTransportador(transportadorId);
        return Ok(disponibilidades);
    }


    [AllowAnonymous]
    [HttpGet("{transportadorId}/horarios")]
    public async Task<IActionResult> ListarHorarios(int transportadorId, [FromQuery] DateTime data)
    {
        var horarios = await _service.ListarHorariosDisponiveis(transportadorId, data);

        return Ok(horarios);
    }


    [HttpPost]
    public async Task<IActionResult> Criar(DisponibilidadeDTO dto)
    {
        var transportadorId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier).Value
        );

        await _service.CriarDisponibilidade(transportadorId, dto);

        return Ok();
    }


    // Atualizar um horário específico
    [Authorize(Roles = "Transportador")]
    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarDisponibilidade(int id, [FromBody] AtualizarDisponibilidadeDTO dto)
    {
        var transportadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var sucesso = await _service.AtualizarDisponibilidade(id, dto, transportadorId);
        if (!sucesso)
            return NotFound();
        return NoContent();
    }

    // Deletar um horário específico
    [Authorize(Roles = "Transportador")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarDisponibilidade(int id)
    {
        var transportadorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var sucesso = await _service.DeletarDisponibilidade(id, transportadorId);
        if (!sucesso)
            return NotFound();
        return NoContent();
    }



   
}
