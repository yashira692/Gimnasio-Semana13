using AutoMapper;
using Gimnasio.Datos.Repositorios;
using Gimnasio.Entidades.DTOs;
using Gimnasio.Entidades.Models;
using Gimnasio.Negocio.Excepciones;

namespace Gimnasio.Negocio.Servicios;

public class SocioService : ISocioService
{
    private readonly ISocioRepository _socioRepo;
    private readonly IPlanRepository _planRepo;
    private readonly IMapper _mapper;

    public SocioService(ISocioRepository socioRepo, IPlanRepository planRepo, IMapper mapper)
    {
        _socioRepo = socioRepo;
        _planRepo = planRepo;
        _mapper = mapper;
    }

    public async Task<List<SocioDto>> ListarAsync(int? planId, string? nombre, bool? activo)
    {
        var socios = await _socioRepo.ObtenerTodosAsync(planId, nombre, activo);
        return _mapper.Map<List<SocioDto>>(socios);
    }

    public async Task<SocioDto?> ObtenerAsync(int id)
    {
        var socio = await _socioRepo.ObtenerPorIdAsync(id);
        return socio is null ? null : _mapper.Map<SocioDto>(socio);
    }

    public async Task<SocioDto> CrearAsync(SocioCrearDto dto)
    {
        // Regla 3: el DNI no se puede repetir
        if (await _socioRepo.ExisteDniAsync(dto.Dni))
            throw new ReglaNegocioException($"Ya existe un socio registrado con el DNI {dto.Dni}.");

        // Regla 4: solo se puede inscribir en un plan existente y activo
        await ValidarPlanActivoAsync(dto.PlanId);

        var socio = _mapper.Map<Socio>(dto);
        await _socioRepo.AgregarAsync(socio);

        var creado = await _socioRepo.ObtenerPorIdAsync(socio.SocioId);
        return _mapper.Map<SocioDto>(creado);
    }

    public async Task<bool> ActualizarAsync(int id, SocioActualizarDto dto)
    {
        var socio = await _socioRepo.ObtenerPorIdAsync(id);
        if (socio is null) return false;

        if (await _socioRepo.ExisteDniAsync(dto.Dni, id))
            throw new ReglaNegocioException($"Ya existe otro socio registrado con el DNI {dto.Dni}.");

        if (dto.PlanId != socio.PlanId)
            await ValidarPlanActivoAsync(dto.PlanId);

        _mapper.Map(dto, socio);   // actualiza el objeto existente
        await _socioRepo.ActualizarAsync(socio);
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var socio = await _socioRepo.ObtenerPorIdAsync(id);
        if (socio is null) return false;

        await _socioRepo.EliminarAsync(socio);
        return true;
    }

    private async Task ValidarPlanActivoAsync(int planId)
    {
        var plan = await _planRepo.ObtenerPorIdAsync(planId);
        if (plan is null)
            throw new ReglaNegocioException($"El plan con id {planId} no existe.");
        if (!plan.Activo)
            throw new ReglaNegocioException($"El plan '{plan.Nombre}' está inactivo y no acepta nuevas inscripciones.");
    }
}
