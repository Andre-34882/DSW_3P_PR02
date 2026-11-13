using System.ComponentModel.DataAnnotations;

namespace DSW_3P_PR02.Models;

public sealed class EventoTecnologico
{
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Categoria { get; set; } = string.Empty;

    public DateTimeOffset Fecha { get; set; }

    [Required, StringLength(200)]
    public string Lugar { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int CupoMaximo { get; set; }

    public bool Activo { get; set; }
}
