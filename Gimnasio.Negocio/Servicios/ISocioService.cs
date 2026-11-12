using Gimnasio.Entidades.DTOs;

namespace Gimnasio.Negocio.Servicios;

public interface ISocioService
{
    Task<List<SocioDto>> ListarAsync(int? planId, string? nombre, bool? activo);
    Task<SocioDto?> ObtenerAsync(int id);
    Task<SocioDto> CrearAsync(SocioCrearDto dto);
    Task<bool> ActualizarAsync(int id, SocioActualizarDto dto);
    Task<bool> EliminarAsync(int id);
}
