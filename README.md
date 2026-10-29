# TrackPoint

API REST para la gestión de vehículos de una flota, desarrollada con ASP.NET Core y Entity Framework Core. El proyecto utiliza MySQL como base de datos y cuenta con pruebas automatizadas para verificar el comportamiento de los endpoints.

> **Estado del proyecto:** en desarrollo.

## Tecnologías

- **C# y .NET 10** — lenguaje y plataforma de desarrollo.
- **ASP.NET Core Web API** — creación de endpoints REST.
- **Entity Framework Core** — acceso y persistencia de datos.
- **MySQL** — sistema gestor de base de datos.
- **xUnit** — pruebas automatizadas.
- **SQLite en memoria** — base de datos aislada para las pruebas de integración.

## Funcionalidades

- Consultar todos los vehículos registrados.
- Obtener los detalles de un vehículo por su identificador.
- Registrar nuevos vehículos.
- Actualizar los datos de un vehículo.
- Eliminar vehículos.
- Validar los datos de entrada.
- Detectar matrículas duplicadas.
- Utilizar códigos de estado HTTP adecuados.
- Separar los controladores de la lógica de negocio mediante una capa de servicios.
- Verificar el comportamiento de la API mediante pruebas automatizadas.

## Estructura del proyecto

```text
TrackPoint/
├── TrackPoint.Api/
│   ├── Controllers/    # Endpoints de la API
│   ├── DTOs/           # Objetos de transferencia de datos
│   ├── Data/           # Contexto de Entity Framework Core
│   ├── Models/         # Entidades del dominio
│   ├── Services/       # Lógica de negocio
│   └── Program.cs      # Configuración de la aplicación
├── TrackPoint.Tests/   # Pruebas automatizadas
└── TrackPoint.sln      # Solución de .NET
```

## Requisitos previos

Antes de ejecutar el proyecto, necesitas:

- [SDK de .NET 10](https://dotnet.microsoft.com/download/dotnet/10.0).
- [MySQL Server](https://dev.mysql.com/downloads/mysql/).
- Git, para clonar el repositorio.

## Configuración

### 1. Clonar el repositorio

```bash
git clone https://github.com/juanmariabravo/trackpoint
cd TrackPoint
```

### 2. Configurar la conexión a MySQL

La API utiliza **User Secrets** para mantener las credenciales fuera del código fuente.

Desde la carpeta `TrackPoint.Api`, ejecuta:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:TrackPoint" "Server=localhost;Port=3306;Database=trackpoint;User ID=TU_USUARIO;Password=TU_CONTRASEÑA;"
```

Sustituye `TU_USUARIO` y `TU_CONTRASEÑA` por las credenciales de tu instalación de MySQL.

> No subas contraseñas, cadenas de conexión reales ni archivos de secretos a GitHub.

### 3. Crear la base de datos y aplicar las migraciones

La API no crea la base de datos ni aplica las migraciones automáticamente al iniciar. El proyecto ya incluye la migración inicial, que puedes aplicar con `dotnet ef`.

Si la base de datos `trackpoint` todaví­a no existe, créala desde MySQL:

```sql
CREATE DATABASE trackpoint;
```

Instala la herramienta de Entity Framework Core si todavía no está instalada:

```bash
dotnet tool install --global dotnet-ef
```

Si ya la tienes instalada, puedes actualizarla con:

```bash
dotnet tool update --global dotnet-ef
```

Desde la carpeta raíz de la solución, aplica las migraciones:

```bash
dotnet ef database update --project TrackPoint.Api --startup-project TrackPoint.Api
```

Este comando aplica las migraciones existentes y crea las tablas definidas por el modelo.

## Ejecución de la API

Desde la carpeta raíz de la solución, compila el proyecto:

```bash
dotnet build
```

Inicia la API:

```bash
dotnet run --project TrackPoint.Api --urls http://localhost:5080
```

Una vez iniciada, la API estará disponible en:

**http://localhost:5080**

Puedes probar los endpoints con la extensión REST Client de Visual Studio Code utilizando el archivo `TrackPoint.Api.http` dentro de `TrackPoint.Api`.

## Endpoints disponibles

| Método | Endpoint | Descripción |
|---|---|---|
| `GET` | `/api/vehicles` | Obtener todos los vehículos |
| `GET` | `/api/vehicles/{id}` | Obtener un vehículo por ID |
| `POST` | `/api/vehicles` | Registrar un vehículo |
| `PUT` | `/api/vehicles/{id}` | Actualizar un vehículo |
| `DELETE` | `/api/vehicles/{id}` | Eliminar un vehículo |

### Ejemplo: registrar un vehículo

Petición `POST /api/vehicles`:

```json
{
  "name": "Mercedes Benz 01",
  "licensePlate": "1234ABC"
}
```

La API valida los datos, normaliza la matrícula y comprueba si ya existe. Si el registro se crea correctamente, devuelve `201 Created`.

## Pruebas automatizadas

El proyecto incluye pruebas automatizadas con xUnit y pruebas de integración de la API.

Para ejecutar todas las pruebas, desde la carpeta raíz:

```bash
dotnet test
```

Las pruebas utilizan SQLite en memoria para evitar depender de una instancia de MySQL durante su ejecución.

> Si la API iniciada con `dotnet run` está provocando conflictos de compilación o archivos bloqueados, detenla con `Ctrl+C` antes de ejecutar las pruebas. Los tests de integración utilizan su propio servidor de pruebas.

## Arquitectura

El proyecto separa las responsabilidades en distintas capas:

- **Controllers:** reciben las peticiones HTTP y generan las respuestas.
- **Services:** implementan la lógica de negocio.
- **DTOs:** definen los datos que entran y salen de la API.
- **Data:** configura el acceso a la base de datos mediante Entity Framework Core.
- **Models:** representan las entidades del dominio.

Esta separación facilita el mantenimiento, las pruebas y la evolución del proyecto.

## Próximas mejoras

- Ampliar la cobertura de las pruebas automatizadas.
- Mejorar el manejo de errores y conflictos de concurrencia.
- Añadir autenticación y autorización.
- Incorporar el registro de posiciones GPS de los vehículos.
- Desarrollar una aplicación móvil con .NET MAUI para consultar y gestionar la flota.

## Autor

**Juan María Bravo López**

- [GitHub](https://github.com/juanmariabravo)
- [Portfolio](https://juanmariabravo.github.io/)

---

Proyecto personal desarrollado con fines de aprendizaje y práctica de tecnologías del ecosistema .NET.