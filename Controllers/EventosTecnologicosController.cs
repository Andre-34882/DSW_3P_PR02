using DSW_3P_PR02.DTOs;
using DSW_3P_PR02.Services;
using Microsoft.AspNetCore.Mvc;

namespace DSW_3P_PR02.Controllers;

[ApiController]
[Route("api/eventos")]
public sealed class EventosTecnologicosController(IEventoTecnologicoService service) : ControllerBase
{
    /// <summary>Obtiene todos los eventos tecnológicos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<EventoTecnologicoResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<EventoTecnologicoResponse>>> ObtenerTodas(
        CancellationToken cancellationToken) =>
        Ok(await service.ObtenerTodasAsync(cancellationToken));

    // Obtiene un evento por su identificador.
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(EventoTecnologicoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EventoTecnologicoResponse>> ObtenerPorId(
        int id,
        CancellationToken cancellationToken)
    {
        var evento = await service.ObtenerPorIdAsync(id, cancellationToken);
        return evento is null ? NotFound() : Ok(evento);
    }

    //Registra un evento tecnológico.
    [HttpPost]
    [ProducesResponseType(typeof(EventoTecnologicoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EventoTecnologicoResponse>> Agregar(
        [FromBody] CreateEventoTecnologicoRequest request,
        CancellationToken cancellationToken)
    {
        var creado = await service.AgregarAsync(request, cancellationToken);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
    }

    //Actualiza todos los datos de un evento tecnológico.
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(
        int id,
        [FromBody] UpdateEventoTecnologicoRequest request,
        CancellationToken cancellationToken)
    {
        var actualizado = await service.ActualizarAsync(id, request, cancellationToken);
        return actualizado ? NoContent() : NotFound();
    }

    /// <summary>Elimina un evento tecnológico.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id, CancellationToken cancellationToken)
    {
        var eliminado = await service.EliminarAsync(id, cancellationToken);
        return eliminado ? NoContent() : NotFound();
    }
}
