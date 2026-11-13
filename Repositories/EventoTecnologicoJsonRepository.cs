using System.Text.Json;
using DSW_3P_PR02.Models;

namespace DSW_3P_PR02.Repositories;

public sealed class EventoTecnologicoJsonRepository : IEventoTecnologicoRepository
{
    private readonly string _rutaArchivo;
    private readonly SemaphoreSlim _semaforo = new(1, 1);
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public EventoTecnologicoJsonRepository(IWebHostEnvironment environment)
    {
        var directorio = Path.Combine(environment.ContentRootPath, "Data");
        Directory.CreateDirectory(directorio);
        _rutaArchivo = Path.Combine(directorio, "eventosTecnologicos.json");
        if (!File.Exists(_rutaArchivo))
            File.WriteAllText(_rutaArchivo, "[]");
    }

    public async Task<IReadOnlyList<EventoTecnologico>> ObtenerTodasAsync(
        CancellationToken cancellationToken)
    {
        await _semaforo.WaitAsync(cancellationToken);
        try { return await LeerAsync(cancellationToken); }
        finally { _semaforo.Release(); }
    }

    public async Task<EventoTecnologico?> ObtenerPorIdAsync(
        int id,
        CancellationToken cancellationToken)
    {
        await _semaforo.WaitAsync(cancellationToken);
        try { return (await LeerAsync(cancellationToken)).FirstOrDefault(e => e.Id == id); }
        finally { _semaforo.Release(); }
    }

    public async Task<EventoTecnologico> AgregarAsync(
        EventoTecnologico eventoTecnologico,
        CancellationToken cancellationToken)
    {
        await _semaforo.WaitAsync(cancellationToken);
        try
        {
            var eventos = await LeerAsync(cancellationToken);
            eventoTecnologico.Id = eventos.Count == 0 ? 1 : eventos.Max(e => e.Id) + 1;
            eventos.Add(eventoTecnologico);
            await GuardarAsync(eventos, cancellationToken);
            return eventoTecnologico;
        }
        finally { _semaforo.Release(); }
    }

    public async Task<bool> ActualizarAsync(
        int id,
        EventoTecnologico eventoTecnologico,
        CancellationToken cancellationToken)
    {
        await _semaforo.WaitAsync(cancellationToken);
        try
        {
            var eventos = await LeerAsync(cancellationToken);
            var indice = eventos.FindIndex(e => e.Id == id);
            if (indice < 0) return false;
            eventoTecnologico.Id = id;
            eventos[indice] = eventoTecnologico;
            await GuardarAsync(eventos, cancellationToken);
            return true;
        }
        finally { _semaforo.Release(); }
    }

    public async Task<bool> EliminarAsync(int id, CancellationToken cancellationToken)
    {
        await _semaforo.WaitAsync(cancellationToken);
        try
        {
            var eventos = await LeerAsync(cancellationToken);
            var evento = eventos.FirstOrDefault(e => e.Id == id);
            if (evento is null) return false;
            eventos.Remove(evento);
            await GuardarAsync(eventos, cancellationToken);
            return true;
        }
        finally { _semaforo.Release(); }
    }

    private async Task<List<EventoTecnologico>> LeerAsync(CancellationToken cancellationToken)
    {
        await using var stream = File.OpenRead(_rutaArchivo);
        return await JsonSerializer.DeserializeAsync<List<EventoTecnologico>>(
            stream, cancellationToken: cancellationToken) ?? [];
    }

    private async Task GuardarAsync(
        List<EventoTecnologico> eventos,
        CancellationToken cancellationToken)
    {
        var temporal = _rutaArchivo + ".tmp";
        await using (var stream = File.Create(temporal))
        {
            await JsonSerializer.SerializeAsync(stream, eventos, JsonOptions, cancellationToken);
        }

        File.Move(temporal, _rutaArchivo, overwrite: true);
    }
}
