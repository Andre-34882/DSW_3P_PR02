using System.Text.Json;
using DSW_3P_PR02.Models;

namespace DSW_3P_PR02.Repositories;

public class EventoTecnologicoJsonRepository : IEventoTecnologicoRepository
{
    private readonly string _rutaArchivo;
    private readonly object _lock = new();
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public EventoTecnologicoJsonRepository(IWebHostEnvironment environment)
    {
        var directorio = Path.Combine(environment.ContentRootPath, "Data");
        Directory.CreateDirectory(directorio);
        _rutaArchivo = Path.Combine(directorio, "eventoTecnologico.json");
        if (!File.Exists(_rutaArchivo))
            File.WriteAllText(_rutaArchivo, "[]");
    }

    public List<EventoTecnologico> ObtenerTodas()
    {
        lock (_lock) return Leer();
    }

    public EventoTecnologico? ObtenerPorId(int id)
    {
        lock (_lock) return Leer().FirstOrDefault(e => e.Id == id);
    }

    public EventoTecnologico Agregar(EventoTecnologico eventoTecnologico)
    {
        lock (_lock)
        {
            var misiones = Leer();
            eventoTecnologico.Id = eventoTecnologicos.Count == 0 ? 1 : eventoTecnologicos.Max(e => e.Id) + 1;
            eventoTecnologicos.Add(eventoTecnologico);
            Guardar(misiones);
            return eventoTecnologico;
        }
    }

    public bool Actualizar(int id, EventoTecnologico eventoTecnologico)
    {
        lock (_lock)
        {
            var misiones = Leer();
            var indice = misiones.FindIndex(m => m.Id == id);
            if (indice < 0) return false;
            mision.Id = id;
            misiones[indice] = eventoTecnologico;
            Guardar(misiones);
            return true;
        }
    }

    public bool Eliminar(int id)
    {
        lock (_lock)
        {
            var misiones = Leer();
            var mision = misiones.FirstOrDefault(m => m.Id == id);
            if (mision is null) return false;
            misiones.Remove(mision);
            Guardar(misiones);
            return true;
        }
    }

    private List<Mision> Leer()
    {
        var json = File.ReadAllText(_rutaArchivo);
        return JsonSerializer.Deserialize<List<Mision>>(json) ?? [];
    }

    private void Guardar(List<Mision> misiones)
    {
        var temporal = _rutaArchivo + ".tmp";
        File.WriteAllText(temporal, JsonSerializer.Serialize(misiones, JsonOptions));
        File.Move(temporal, _rutaArchivo, overwrite: true);
    }
}
