using DSW_3P_PR02.Models;

namespace DSW_3P_PR02.Repositories;

public interface IEventoTecnologicoRepository
{
    Task<IReadOnlyList<EventoTecnologico>> ObtenerTodasAsync(CancellationToken cancellationToken);
    Task<EventoTecnologico?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken);
    Task<EventoTecnologico> AgregarAsync(
        EventoTecnologico eventoTecnologico,
        CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(
        int id,
        EventoTecnologico eventoTecnologico,
        CancellationToken cancellationToken);
    Task<bool> EliminarAsync(int id, CancellationToken cancellationToken);
}
