using DSW_3P_PR02.Models;

namespace DSW_3P_PR01.Repositories;

public interface IEventoTecnologicoRepository
{
    List<EventoTecnologico> ObtenerTodas();
    EventoTecnologico? ObtenerPorId(int id);
    EventoTecnologico Agregar(EventoTecnologico eventoTecnologico);
    bool Actualizar(int id, EventoTecnologico eventoTecnologico);
    bool Eliminar(int id);
}
