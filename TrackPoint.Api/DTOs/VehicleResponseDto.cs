namespace TrackPoint.Api.DTOs;

public class VehicleResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string LicensePlate { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}
