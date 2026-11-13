using System.ComponentModel.DataAnnotations;

namespace DSW_3P_PR02.DTOs;

/// <summary>Datos para reemplazar la información de un evento tecnológico.</summary>
public sealed record UpdateEventoTecnologicoRequest
{
    [Required, StringLength(150)]
    public required string Nombre { get; init; }

    [Required, StringLength(100)]
    public required string Categoria { get; init; }

    public required DateTimeOffset Fecha { get; init; }

    [Required, StringLength(200)]
    public required string Lugar { get; init; }

    [Range(1, int.MaxValue)]
    public required int CupoMaximo { get; init; }

    public required bool Activo { get; init; }
}
