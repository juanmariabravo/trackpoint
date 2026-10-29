## Cómo se creó el proyecto de pruebas. Evolución y cambios

### 1. Crear el proyecto de pruebas

dotnet new xunit -n TrackPoint.Tests

### 2. Añadirlo a la solución

dotnet sln add .\TrackPoint.Tests\TrackPoint.Tests.csproj

### 3. Añadir una referencia a la API

dotnet add .\TrackPoint.Tests\TrackPoint.Tests.csproj reference .\TrackPoint.Api\TrackPoint.Api.csproj

### 4. Ejecutar las pruebas iniciales

dotnet test

### 5. Instalar el paquete de Microsoft.AspNetCore.Mvc.Testing
```
(cd TrackPoint.Tests)
dotnet add package Microsoft.AspNetCore.Mvc.Testing --version 10.0.14
```

### 6. Configurar un entorno de pruebas separado

No queremos que las pruebas borren o modifiquen los datos de tu base de datos de desarrollo.

Para ello, utilizaremos una base de datos SQLite en memoria:
```
(cd TrackPoint))
dotnet add .\TrackPoint.Tests\TrackPoint.Tests.csproj package Microsoft.EntityFrameworkCore.Sqlite --version 10.0.14
```

Prepararemos un punto de entrada para la API que nos permita configurar el entorno de pruebas. Para ello, añadiremos una clase parcial `Program` en `Program.cs` después de `app.Run()`;

```csharp
public partial class Program { }
```

**Configuraremos WebApplicationFactory para sustituir el TrackPointDbContext de MySQL por SQLite durante las pruebas.**
Creamos la fábrica de pruebas (`TrackPoint.Tests/TrackPointApiFactory.cs`) que arranca la API, sustituye la conexión MySQL por SQLite en memoria y mantiene abierta la conexión mientras duran las pruebas.

> **¿Qué hace esta clase?**
> - `WebApplicationFactory<Program>` inicia la aplicación de prueba sin que tengas que ejecutar dotnet run.
> - `SqliteConnection` crea una base de datos en memoria.
> - `RemoveAll` elimina las configuraciones del DbContext que utilizaban MySQL.
> - `UseSqlite` configura el proveedor que utilizarán las pruebas.

La base de datos en memoria desaparecerá al cerrar su conexión. No se modificará la base de datos trackpoint de MySQL.

> [!alert] Es importante que la API no esta corriendo mientras se ejecutan las pruebas de integración. Si la API está corriendo, las pruebas no funcionarán correctamente.

### 7. Crearemos el cliente HTTP y nuestra primera prueba de integración
`TrackPoint.Tests\VehiclesApiTests.cs` contiene la prueba de integración que arranca la API, crea un cliente HTTP y realiza una petición GET a `/api/vehicles`. La prueba comprueba que la respuesta es correcta y que devuelve un código 200 OK.