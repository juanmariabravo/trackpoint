
using TrackPoint.Api.DTOs;

namespace TrackPoint.Api.Services;

public interface IVehicleService
{
    Task<List<VehicleResponseDto>> GetAllAsync();

    Task<VehicleResponseDto?> GetByIdAsync(int id);

    Task<VehicleResponseDto> CreateAsync(
        VehicleCreateDto dto);

    Task<VehicleResponseDto?> UpdateAsync(
        int id,
        VehicleUpdateDto dto);

    Task<bool> DeleteAsync(int id);

    Task<bool> LicensePlateExistsAsync(
        string licensePlate,
        int? excludeVehicleId = null);
}
