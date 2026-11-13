using DSW_3P_PR02.DTOs;
using DSW_3P_PR02.Models;
using DSW_3P_PR02.Repositories;

namespace DSW_3P_PR02.Services;

public sealed class EventoTecnologicoService(IEventoTecnologicoRepository repository)
    : IEventoTecnologicoService
{
    public async Task<IReadOnlyList<EventoTecnologicoResponse>> ObtenerTodasAsync(
        CancellationToken cancellationToken)
    {
        var eventos = await repository.ObtenerTodasAsync(cancellationToken);
        return eventos.Select(Mapear).ToList();
    }

    public async Task<EventoTecnologicoResponse?> ObtenerPorIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var evento = await repository.ObtenerPorIdAsync(id, cancellationToken);
        return evento is null ? null : Mapear(evento);
    }

    public async Task<EventoTecnologicoResponse> AgregarAsync(
        CreateEventoTecnologicoRequest request,
        CancellationToken cancellationToken)
    {
        Validar(request.Nombre, request.Categoria, request.Fecha, request.Lugar, request.CupoMaximo);
        var evento = new EventoTecnologico
        {
            Nombre = request.Nombre.Trim(),
            Categoria = request.Categoria.Trim(),
            Fecha = request.Fecha,
            Lugar = request.Lugar.Trim(),
            CupoMaximo = request.CupoMaximo,
            Activo = request.Activo
        };

        return Mapear(await repository.AgregarAsync(evento, cancellationToken));
    }

    public async Task<bool> ActualizarAsync(
        int id,
        UpdateEventoTecnologicoRequest request,
        CancellationToken cancellationToken)
    {
        Validar(request.Nombre, request.Categoria, request.Fecha, request.Lugar, request.CupoMaximo);
        var evento = new EventoTecnologico
        {
            Id = id,
            Nombre = request.Nombre.Trim(),
            Categoria = request.Categoria.Trim(),
            Fecha = request.Fecha,
            Lugar = request.Lugar.Trim(),
            CupoMaximo = request.CupoMaximo,
            Activo = request.Activo
        };

        return await repository.ActualizarAsync(id, evento, cancellationToken);
    }

    public Task<bool> EliminarAsync(int id, CancellationToken cancellationToken) =>
        repository.EliminarAsync(id, cancellationToken);

    private static void Validar(
        string? nombre,
        string? categoria,
        DateTimeOffset fecha,
        string? lugar,
        int cupoMaximo)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre del evento es obligatorio.");
        if (string.IsNullOrWhiteSpace(categoria))
            throw new ArgumentException("La categoría del evento es obligatoria.");
        if (fecha == default)
            throw new ArgumentException("La fecha del evento debe ser válida.");
        if (string.IsNullOrWhiteSpace(lugar))
            throw new ArgumentException("El lugar del evento es obligatorio.");
        if (cupoMaximo <= 0)
            throw new ArgumentException("El cupo máximo debe ser mayor que cero.");
    }

    private static EventoTecnologicoResponse Mapear(EventoTecnologico evento) =>
        new(
            evento.Id,
            evento.Nombre,
            evento.Categoria,
            evento.Fecha,
            evento.Lugar,
            evento.CupoMaximo,
            evento.Activo);
}