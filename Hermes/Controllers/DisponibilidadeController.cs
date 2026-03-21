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

    [HttpPost]
    public async Task<IActionResult> Criar(DisponibilidadeDTO dto)
    {
        var transportadorId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier).Value
        );

        await _service.CriarDisponibilidade(transportadorId, dto);

        return Ok();
    }

    [AllowAnonymous]
    [HttpGet("{transportadorId}/horarios")]
    public async Task<IActionResult> ListarHorarios(int transportadorId, [FromQuery] DateTime data)
    {
        var horarios = await _service.ListarHorariosDisponiveis(transportadorId, data);

        return Ok(horarios);
    }
}
