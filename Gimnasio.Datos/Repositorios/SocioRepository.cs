using Gimnasio.Datos.Models;
using Gimnasio.Entidades.Models;
using Microsoft.EntityFrameworkCore;

namespace Gimnasio.Datos.Repositorios;

public class SocioRepository : ISocioRepository
{
    private readonly GimnasioContext _context;

    public SocioRepository(GimnasioContext context)
    {
        _context = context;
    }

    // GET con filtros opcionales (query string)
    public async Task<List<Socio>> ObtenerTodosAsync(int? planId, string? nombre, bool? activo)
    {
        IQueryable<Socio> consulta = _context.Set<Socio>().Include(s => s.Plan);

        if (planId.HasValue)
            consulta = consulta.Where(s => s.PlanId == planId.Value);

        if (!string.IsNullOrWhiteSpace(nombre))
            consulta = consulta.Where(s => s.Nombres.Contains(nombre) || s.Apellidos.Contains(nombre));

        if (activo.HasValue)
            consulta = consulta.Where(s => s.Activo == activo.Value);

        return await consulta.OrderBy(s => s.Apellidos).ToListAsync();
    }

    public async Task<Socio?> ObtenerPorIdAsync(int id)
    {
        return await _context.Set<Socio>()
            .Include(s => s.Plan)
            .FirstOrDefaultAsync(s => s.SocioId == id);
    }

    public async Task<List<Socio>> ObtenerPorPlanAsync(int planId)
    {
        return await _context.Set<Socio>()
            .Include(s => s.Plan)
            .Where(s => s.PlanId == planId)
            .OrderBy(s => s.Apellidos)
            .ToListAsync();
    }

    public async Task<bool> ExisteDniAsync(string dni, int? excluirId = null)
    {
        return await _context.Set<Socio>()
            .AnyAsync(s => s.Dni == dni && (excluirId == null || s.SocioId != excluirId));
    }

    public async Task AgregarAsync(Socio socio)
    {
        _context.Set<Socio>().Add(socio);
        await _context.SaveChangesAsync();
    }

    public async Task ActualizarAsync(Socio socio)
    {
        // La entidad ya viene rastreada (se obtuvo con ObtenerPorIdAsync y AutoMapper la modificó)
        if (_context.Entry(socio).State == EntityState.Detached)
            _context.Set<Socio>().Update(socio);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Socio socio)
    {
        _context.Set<Socio>().Remove(socio);
        await _context.SaveChangesAsync();
    }
}
