using AutoMapper;
using Gimnasio.Entidades.DTOs;
using Gimnasio.Entidades.Models;

namespace Gimnasio.Negocio.Mapeos;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ===== PLAN =====
        // Entidad -> DTO de lectura (lo que devuelve la API)
        CreateMap<Plan, PlanDto>()
            .ForMember(d => d.CantidadSocios, o => o.MapFrom(s => s.Socios.Count));

        // DTO de entrada -> Entidad (crear): todo plan nuevo nace activo
        CreateMap<PlanCrearDto, Plan>()
            .ForMember(d => d.Activo, o => o.MapFrom(_ => true));

        // DTO de entrada -> Entidad existente (actualizar con Map(origen, destino))
        CreateMap<PlanActualizarDto, Plan>()
            .ForMember(d => d.PlanId, o => o.Ignore());

        // ===== SOCIO =====
        // Entidad -> DTO de lectura: devuelve el nombre del plan en vez del objeto Plan
        CreateMap<Socio, SocioDto>()
            .ForMember(d => d.NombreCompleto, o => o.MapFrom(s => s.Nombres + " " + s.Apellidos))
            .ForMember(d => d.Plan, o => o.MapFrom(s => s.Plan.Nombre));

        // DTO de entrada -> Entidad (crear): fecha de hoy y socio activo
        CreateMap<SocioCrearDto, Socio>()
            .ForMember(d => d.FechaInscripcion, o => o.MapFrom(_ => DateOnly.FromDateTime(DateTime.Today)))
            .ForMember(d => d.Activo, o => o.MapFrom(_ => true));

        // DTO de entrada -> Entidad existente (actualizar): se conserva Id y fecha de inscripción
        CreateMap<SocioActualizarDto, Socio>()
            .ForMember(d => d.SocioId, o => o.Ignore())
            .ForMember(d => d.FechaInscripcion, o => o.Ignore());
    }
}
