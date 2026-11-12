using Gimnasio.Entidades.Models;

namespace Gimnasio.Datos.Repositorios;

public interface IPlanRepository
{
    Task<List<Plan>> ObtenerTodosAsync();
    Task<Plan?> ObtenerPorIdAsync(int id);
    Task<bool> ExisteNombreAsync(string nombre, int? excluirId = null);
    Task<bool> TieneSociosAsync(int id);
    Task AgregarAsync(Plan plan);
    Task ActualizarAsync(Plan plan);
    Task EliminarAsync(Plan plan);
}
