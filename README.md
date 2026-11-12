# Gimnasio API — SEMANA13 (API en capas con DTOs y AutoMapper)

Web API REST en capas con **ASP.NET Core (.NET 10)**, **Entity Framework Core 10** (base de datos primero) y **AutoMapper**.

**Tema:** gestión de un gimnasio. Un **Plan** de membresía (Básico, Premium, Black…) tiene muchos **Socios** (relación 1:N).

## Arquitectura
| Proyecto | Contenido |
|---|---|
| `Gimnasio.Entidades` | Entidades (`Plan`, `Socio`) generadas con Scaffold y DTOs |
| `Gimnasio.Datos` | `GimnasioContext` y repositorios (`IPlanRepository`, `ISocioRepository`) |
| `Gimnasio.Negocio` | Servicios, reglas de negocio, `ReglaNegocioException` y `MappingProfile` |
| `Gimnasio.Api` | Controladores (`PlanesController`, `SociosController`) y `Program.cs` |

Referencias: `Api → Negocio → Datos → Entidades`.

## Reglas de negocio (409 Conflict)
1. No pueden existir dos planes con el mismo nombre.
2. No se puede eliminar un plan que tiene socios inscritos.
3. El DNI de un socio no se puede repetir.
4. Solo se puede inscribir a un socio en un plan existente y activo.

## Endpoints
| Método | Ruta |
|---|---|
| GET | `/api/planes` |
| GET | `/api/planes/{id}` |
| GET | `/api/planes/{id}/socios` (ruta anidada) |
| POST | `/api/planes` |
| PUT | `/api/planes/{id}` |
| DELETE | `/api/planes/{id}` |
| GET | `/api/socios?planId=&nombre=&activo=` (filtros) |
| GET | `/api/socios/{id}` |
| POST | `/api/socios` |
| PUT | `/api/socios/{id}` |
| DELETE | `/api/socios/{id}` |

## Cómo ejecutar
1. Ejecutar `database/GimnasioDB.sql` en SQL Server.
2. Guardar la cadena de conexión en User Secrets (no está en el repositorio):
   ```
   dotnet user-secrets set "ConnectionStrings:GimnasioDB" "Server=(localdb)\MSSQLLocalDB;Database=GimnasioDB;Trusted_Connection=True;TrustServerCertificate=True" --project Gimnasio.Api
   ```
3. `dotnet run --project Gimnasio.Api` y abrir `http://localhost:5080/swagger`.
