using DSW_3P_PR02.DTOs;

namespace DSW_3P_PR02.Services;

public interface IEventoTecnologicoService
{
    Task<IReadOnlyList<EventoTecnologicoResponse>> ObtenerTodasAsync(
        CancellationToken cancellationToken);
    Task<EventoTecnologicoResponse?> ObtenerPorIdAsync(
        int id,
        CancellationToken cancellationToken);
    Task<EventoTecnologicoResponse> AgregarAsync(
        CreateEventoTecnologicoRequest request,
        CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(
        int id,
        UpdateEventoTecnologicoRequest request,
        CancellationToken cancellationToken);
    Task<bool> EliminarAsync(int id, CancellationToken cancellationToken);
}