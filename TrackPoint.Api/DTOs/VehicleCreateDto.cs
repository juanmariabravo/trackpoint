using System.ComponentModel.DataAnnotations;

namespace TrackPoint.Api.DTOs;

public class VehicleCreateDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "La matrícula es obligatoria.")]
    [StringLength(20)]
    public string LicensePlate { get; set; } = string.Empty;
}
