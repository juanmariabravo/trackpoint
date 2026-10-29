
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TrackPoint.Api.Data;

namespace TrackPoint.Tests;

public class TrackPointApiFactory : WebApplicationFactory<Program>
{
    private readonly SqliteConnection _connection =
        new("Data Source=:memory:");

    public TrackPointApiFactory()
    {
        _connection.Open();
    }

    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Eliminar la configuración del proveedor MySQL.
            services.RemoveAll<
                IDbContextOptionsConfiguration<TrackPointDbContext>>();

            services.RemoveAll<
                DbContextOptions<TrackPointDbContext>>();

            // Usar SQLite para las pruebas.
            services.AddDbContext<TrackPointDbContext>(
                options => options.UseSqlite(_connection));
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            _connection.Dispose();
        }
    }
}
