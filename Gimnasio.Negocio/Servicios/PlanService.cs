using AutoMapper;
using Gimnasio.Datos.Repositorios;
using Gimnasio.Entidades.DTOs;
using Gimnasio.Entidades.Models;
using Gimnasio.Negocio.Excepciones;

namespace Gimnasio.Negocio.Servicios;

public class PlanService : IPlanService
{
    private readonly IPlanRepository _planRepo;
    private readonly ISocioRepository _socioRepo;
    private readonly IMapper _mapper;

    public PlanService(IPlanRepository planRepo, ISocioRepository socioRepo, IMapper mapper)
    {
        _planRepo = planRepo;
        _socioRepo = socioRepo;
        _mapper = mapper;
    }

    public async Task<List<PlanDto>> ListarAsync()
    {
        var planes = await _planRepo.ObtenerTodosAsync();
        return _mapper.Map<List<PlanDto>>(planes);
    }

    public async Task<PlanDto?> ObtenerAsync(int id)
    {
        var plan = await _planRepo.ObtenerPorIdAsync(id);
        return plan is null ? null : _mapper.Map<PlanDto>(plan);
    }

    public async Task<List<SocioDto>?> ListarSociosDelPlanAsync(int planId)
    {
        var plan = await _planRepo.ObtenerPorIdAsync(planId);
        if (plan is null) return null;
        var socios = await _socioRepo.ObtenerPorPlanAsync(planId);
        return _mapper.Map<List<SocioDto>>(socios);
    }

    public async Task<PlanDto> CrearAsync(PlanCrearDto dto)
    {
        // Regla 1: no pueden existir dos planes con el mismo nombre
        if (await _planRepo.ExisteNombreAsync(dto.Nombre))
            throw new ReglaNegocioException($"Ya existe un plan con el nombre '{dto.Nombre}'.");

        var plan = _mapper.Map<Plan>(dto);
        await _planRepo.AgregarAsync(plan);
        return _mapper.Map<PlanDto>(plan);
    }

    public async Task<bool> ActualizarAsync(int id, PlanActualizarDto dto)
    {
        var plan = await _planRepo.ObtenerPorIdAsync(id);
        if (plan is null) return false;

        if (await _planRepo.ExisteNombreAsync(dto.Nombre, id))
            throw new ReglaNegocioException($"Ya existe otro plan con el nombre '{dto.Nombre}'.");

        _mapper.Map(dto, plan);   // actualiza el objeto existente
        await _planRepo.ActualizarAsync(plan);
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var plan = await _planRepo.ObtenerPorIdAsync(id);
        if (plan is null) return false;

        // Regla 2: no se puede eliminar un plan que tiene socios inscritos
        if (await _planRepo.TieneSociosAsync(id))
            throw new ReglaNegocioException($"No se puede eliminar el plan '{plan.Nombre}' porque tiene socios inscritos.");

        await _planRepo.EliminarAsync(plan);
        return true;
    }
}
