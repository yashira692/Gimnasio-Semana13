using Gimnasio.Datos.Models;
using Gimnasio.Entidades.Models;
using Microsoft.EntityFrameworkCore;

namespace Gimnasio.Datos.Repositorios;

public class PlanRepository : IPlanRepository
{
    private readonly GimnasioContext _context;

    public PlanRepository(GimnasioContext context)
    {
        _context = context;
    }

    public async Task<List<Plan>> ObtenerTodosAsync()
    {
        return await _context.Set<Plan>()
            .Include(p => p.Socios)
            .OrderBy(p => p.PrecioMensual)
            .ToListAsync();
    }

    public async Task<Plan?> ObtenerPorIdAsync(int id)
    {
        return await _context.Set<Plan>()
            .Include(p => p.Socios)
            .FirstOrDefaultAsync(p => p.PlanId == id);
    }

    public async Task<bool> ExisteNombreAsync(string nombre, int? excluirId = null)
    {
        return await _context.Set<Plan>()
            .AnyAsync(p => p.Nombre == nombre && (excluirId == null || p.PlanId != excluirId));
    }

    public async Task<bool> TieneSociosAsync(int id)
    {
        return await _context.Set<Socio>().AnyAsync(s => s.PlanId == id);
    }

    public async Task AgregarAsync(Plan plan)
    {
        _context.Set<Plan>().Add(plan);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Plan plan)
    {
        // La entidad ya viene rastreada (se obtuvo con ObtenerPorIdAsync y AutoMapper la modificó)
        if (_context.Entry(plan).State == EntityState.Detached)
            _context.Set<Plan>().Update(plan);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Plan plan)
    {
        _context.Set<Plan>().Remove(plan);
        await _context.SaveChangesAsync();
    }
}
