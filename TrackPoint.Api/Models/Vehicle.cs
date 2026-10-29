using System.ComponentModel.DataAnnotations;

namespace TrackPoint.Api.Models;

public class Vehicle
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 2,
        ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "La matrícula es obligatoria.")]
    [StringLength(20, ErrorMessage = "La matrícula no puede superar los 20 caracteres.")]
    public string LicensePlate { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}