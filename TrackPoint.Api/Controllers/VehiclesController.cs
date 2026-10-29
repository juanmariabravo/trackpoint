
using Microsoft.AspNetCore.Mvc;
using TrackPoint.Api.DTOs;
using TrackPoint.Api.Services;

namespace TrackPoint.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    // GET: api/vehicles
    [HttpGet]
    public async Task<ActionResult<List<VehicleResponseDto>>> GetAll()
    {
        var vehicles = await _vehicleService.GetAllAsync();

        return Ok(vehicles);
    }

    // GET: api/vehicles/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<VehicleResponseDto>> GetById(int id)
    {
        var vehicle = await _vehicleService.GetByIdAsync(id);

        if (vehicle is null)
        {
            return NotFound();
        }

        return Ok(vehicle);
    }

    // POST: api/vehicles
    [HttpPost]
    public async Task<ActionResult<VehicleResponseDto>> Create(
        VehicleCreateDto dto)
    {
        var name = dto.Name.Trim();
        var licensePlate =
            dto.LicensePlate.Trim().ToUpperInvariant();

        var plateExists =
            await _vehicleService.LicensePlateExistsAsync(licensePlate);

        if (plateExists)
        {
            return Conflict(new
            {
                message = "Ya existe un vehículo con esa matrícula."
            });
        }

        var vehicleToCreate = new VehicleCreateDto
        {
            Name = name,
            LicensePlate = licensePlate
        };

        var createdVehicle =
            await _vehicleService.CreateAsync(vehicleToCreate);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdVehicle.Id },
            createdVehicle);
    }

    // PUT: api/vehicles/1
    [HttpPut("{id:int}")]
    public async Task<ActionResult<VehicleResponseDto>> Update(
        int id,
        VehicleUpdateDto dto)
    {
        var vehicle = await _vehicleService.GetByIdAsync(id);

        if (vehicle is null)
        {
            return NotFound();
        }

        var licensePlate =
            dto.LicensePlate.Trim().ToUpperInvariant();

        var plateExists =
            await _vehicleService.LicensePlateExistsAsync(
                licensePlate,
                excludeVehicleId: id);

        if (plateExists)
        {
            return Conflict(new
            {
                message = "Ya existe otro vehículo con esa matrícula."
            });
        }

        var vehicleToUpdate = new VehicleUpdateDto
        {
            Name = dto.Name.Trim(),
            LicensePlate = licensePlate,
            IsActive = dto.IsActive
        };

        var updatedVehicle =
            await _vehicleService.UpdateAsync(id, vehicleToUpdate);

        if (updatedVehicle is null)
        {
            return NotFound();
        }

        return Ok(updatedVehicle);
    }

    // DELETE: api/vehicles/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _vehicleService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
