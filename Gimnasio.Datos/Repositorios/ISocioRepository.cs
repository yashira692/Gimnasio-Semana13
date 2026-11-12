using Gimnasio.Entidades.Models;

namespace Gimnasio.Datos.Repositorios;

public interface ISocioRepository
{
    Task<List<Socio>> ObtenerTodosAsync(int? planId, string? nombre, bool? activo);
    Task<Socio?> ObtenerPorIdAsync(int id);
    Task<List<Socio>> ObtenerPorPlanAsync(int planId);
    Task<bool> ExisteDniAsync(string dni, int? excluirId = null);
    Task AgregarAsync(Socio socio);
    Task ActualizarAsync(Socio socio);
    Task EliminarAsync(Socio socio);
}
