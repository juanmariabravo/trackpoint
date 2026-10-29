
using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TrackPoint.Api.Data;
using TrackPoint.Api.Models;

namespace TrackPoint.Tests;

public class VehiclesApiTests
    : IClassFixture<TrackPointApiFactory>
{
    private readonly TrackPointApiFactory _factory;

    public VehiclesApiTests(TrackPointApiFactory factory)
    {
        _factory = factory;
    }

    /// <summary>
    /// Limpia la base de datos antes de cada test.
    /// </summary>
    private async Task ResetDatabaseAsync()
    {
        using var scope = _factory.Services.CreateScope();

        var db = scope.ServiceProvider
            .GetRequiredService<TrackPointDbContext>();

        await db.Database.EnsureCreatedAsync();

        await db.Vehicles.ExecuteDeleteAsync();
    }

    /// <summary>
    /// Comprueba que se obtiene una lista vacía
    /// cuando no existen vehículos.
    /// </summary>
    [Fact]
    public async Task GetVehicles_ReturnsOk()
    {
        await ResetDatabaseAsync();

        using var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/vehicles");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var vehicles = await response.Content
            .ReadFromJsonAsync<List<Vehicle>>();

        Assert.NotNull(vehicles);
        Assert.Empty(vehicles);
    }

    /// <summary>
    /// Comprueba que se puede crear un vehículo correctamente.
    /// </summary>
    [Fact]
    public async Task PostVehicle_CreatesVehicleSuccessfully()
    {
        await ResetDatabaseAsync();

        using var client = _factory.CreateClient();

        var vehicleData = new
        {
            Name = "Mercedes Benz 01",
            LicensePlate = "1234ABC"
        };

        var response = await client.PostAsJsonAsync(
            "/api/vehicles", vehicleData);

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        var createdVehicle = await response.Content
            .ReadFromJsonAsync<Vehicle>();

        Assert.NotNull(createdVehicle);
        Assert.Equal(vehicleData.Name, createdVehicle!.Name);
        Assert.Equal(
            vehicleData.LicensePlate,
            createdVehicle.LicensePlate);
        Assert.True(createdVehicle.IsActive);
    }

    /// <summary>
    /// Comprueba que se devuelve 404
    /// cuando el vehículo no existe.
    /// </summary>
    [Fact]
    public async Task GetById_WhenVehicleDoesNotExist_Returns404()
    {
        await ResetDatabaseAsync();

        using var client = _factory.CreateClient();

        var response = await client.GetAsync(
            "/api/vehicles/99999");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    /// <summary>
    /// Comprueba que no se admiten matrículas duplicadas.
    /// </summary>
    [Fact]
    public async Task Create_WhenLicensePlateIsDuplicate_ReturnsConflict()
    {
        await ResetDatabaseAsync();

        using var client = _factory.CreateClient();

        var vehicleData = new
        {
            Name = "Mercedes Benz 01",
            LicensePlate = "1234ABC"
        };

        // Crear el primer vehículo.
        var firstResponse = await client.PostAsJsonAsync(
            "/api/vehicles", vehicleData);

        Assert.Equal(
            HttpStatusCode.Created,
            firstResponse.StatusCode);

        // Intentar crear otro vehículo con la misma matrícula.
        var duplicateData = new
        {
            Name = "Mercedes Benz 02",
            LicensePlate = "1234ABC"
        };

        var duplicateResponse = await client.PostAsJsonAsync(
            "/api/vehicles", duplicateData);

        Assert.Equal(
            HttpStatusCode.Conflict,
            duplicateResponse.StatusCode);
    }
}
