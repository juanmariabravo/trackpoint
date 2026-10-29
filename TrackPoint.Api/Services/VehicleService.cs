
using Microsoft.EntityFrameworkCore;
using TrackPoint.Api.Data;
using TrackPoint.Api.DTOs;
using TrackPoint.Api.Models;

namespace TrackPoint.Api.Services;

public class VehicleService : IVehicleService
{
    private readonly TrackPointDbContext _db;

    public VehicleService(TrackPointDbContext db)
    {
        _db = db;
    }

    public async Task<List<VehicleResponseDto>> GetAllAsync()
    {
        return await _db.Vehicles
            .AsNoTracking()
            .OrderBy(v => v.Id)
            .Select(v => new VehicleResponseDto
            {
                Id = v.Id,
                Name = v.Name,
                LicensePlate = v.LicensePlate,
                IsActive = v.IsActive,
                CreatedAtUtc = v.CreatedAtUtc
            })
            .ToListAsync();
    }

    public async Task<VehicleResponseDto?> GetByIdAsync(int id)
    {
        return await _db.Vehicles
            .AsNoTracking()
            .Where(v => v.Id == id)
            .Select(v => new VehicleResponseDto
            {
                Id = v.Id,
                Name = v.Name,
                LicensePlate = v.LicensePlate,
                IsActive = v.IsActive,
                CreatedAtUtc = v.CreatedAtUtc
            })
            .FirstOrDefaultAsync();
    }

    public async Task<VehicleResponseDto> CreateAsync(
        VehicleCreateDto dto)
    {
        var vehicle = new Vehicle
        {
            Name = dto.Name.Trim(),
            LicensePlate = dto.LicensePlate.Trim().ToUpperInvariant(),
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow
        };

        _db.Vehicles.Add(vehicle);

        await _db.SaveChangesAsync();

        return MapToResponse(vehicle);
    }

    public async Task<VehicleResponseDto?> UpdateAsync(
        int id,
        VehicleUpdateDto dto)
    {
        var vehicle = await _db.Vehicles
            .FirstOrDefaultAsync(v => v.Id == id);

        if (vehicle is null)
        {
            return null;
        }

        vehicle.Name = dto.Name.Trim();
        vehicle.LicensePlate =
            dto.LicensePlate.Trim().ToUpperInvariant();
        vehicle.IsActive = dto.IsActive;

        await _db.SaveChangesAsync();

        return MapToResponse(vehicle);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var vehicle = await _db.Vehicles
            .FirstOrDefaultAsync(v => v.Id == id);

        if (vehicle is null)
        {
            return false;
        }

        _db.Vehicles.Remove(vehicle);

        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> LicensePlateExistsAsync(
        string licensePlate,
        int? excludeVehicleId = null)
    {
        var normalizedPlate =
            licensePlate.Trim().ToUpperInvariant();

        return await _db.Vehicles.AnyAsync(v =>
            v.LicensePlate == normalizedPlate &&
            (!excludeVehicleId.HasValue ||
             v.Id != excludeVehicleId.Value));
    }

    private static VehicleResponseDto MapToResponse(
        Vehicle vehicle)
    {
        return new VehicleResponseDto
        {
            Id = vehicle.Id,
            Name = vehicle.Name,
            LicensePlate = vehicle.LicensePlate,
            IsActive = vehicle.IsActive,
            CreatedAtUtc = vehicle.CreatedAtUtc
        };
    }
}
