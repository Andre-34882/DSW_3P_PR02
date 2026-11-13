namespace DSW_3P_PR02.DTOs;

/// <summary>Representa un evento tecnológico devuelto por la API.</summary>
public sealed record EventoTecnologicoResponse(
    int Id,
    string Nombre,
    string Categoria,
    DateTimeOffset Fecha,
    string Lugar,
    int CupoMaximo,
    bool Activo);
