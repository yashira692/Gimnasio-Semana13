using Gimnasio.Entidades.DTOs;

namespace Gimnasio.Negocio.Servicios;

public interface IPlanService
{
    Task<List<PlanDto>> ListarAsync();
    Task<PlanDto?> ObtenerAsync(int id);
    Task<List<SocioDto>?> ListarSociosDelPlanAsync(int planId);
    Task<PlanDto> CrearAsync(PlanCrearDto dto);
    Task<bool> ActualizarAsync(int id, PlanActualizarDto dto);
    Task<bool> EliminarAsync(int id);
}
