using DSW_3P_PR02.Data;
using DSW_3P_PR02.Models;
using Microsoft.EntityFrameworkCore;

namespace DSW_3P_PR02.Repositories;

public sealed class EventoTecnologicoSqlRepository(EventosDbContext context)
    : IEventoTecnologicoRepository
{
    public async Task<IReadOnlyList<EventoTecnologico>> ObtenerTodasAsync(
        CancellationToken cancellationToken) =>
        await context.Eventos.AsNoTracking().ToListAsync(cancellationToken);

    public Task<EventoTecnologico?> ObtenerPorIdAsync(
        int id,
        CancellationToken cancellationToken) =>
        context.Eventos.AsNoTracking().FirstOrDefaultAsync(
            evento => evento.Id == id, cancellationToken);

    public async Task<EventoTecnologico> AgregarAsync(
        EventoTecnologico eventoTecnologico,
        CancellationToken cancellationToken)
    {
        eventoTecnologico.Id = 0;
        context.Eventos.Add(eventoTecnologico);
        await context.SaveChangesAsync(cancellationToken);
        return eventoTecnologico;
    }

    public async Task<bool> ActualizarAsync(
        int id,
        EventoTecnologico eventoTecnologico,
        CancellationToken cancellationToken)
    {
        var actual = await context.Eventos.FindAsync([id], cancellationToken);
        if (actual is null) return false;

        actual.Nombre = eventoTecnologico.Nombre;
        actual.Categoria = eventoTecnologico.Categoria;
        actual.Fecha = eventoTecnologico.Fecha;
        actual.Lugar = eventoTecnologico.Lugar;
        actual.CupoMaximo = eventoTecnologico.CupoMaximo;
        actual.Activo = eventoTecnologico.Activo;
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> EliminarAsync(int id, CancellationToken cancellationToken)
    {
        var evento = await context.Eventos.FindAsync([id], cancellationToken);
        if (evento is null) return false;

        context.Eventos.Remove(evento);
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
