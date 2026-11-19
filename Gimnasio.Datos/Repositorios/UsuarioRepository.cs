using Gimnasio.Datos.Models;
using Gimnasio.Entidades.Models;
using Microsoft.EntityFrameworkCore;

namespace Gimnasio.Datos.Repositorios;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly GimnasioContext _context;

    public UsuarioRepository(GimnasioContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> BuscarPorCorreoAsync(string correo)
    {
        return await _context.Set<Usuario>()
            .FirstOrDefaultAsync(u => u.Correo == correo);
    }

    public async Task AgregarAsync(Usuario usuario)
    {
        await _context.Set<Usuario>().AddAsync(usuario);
    }

    public async Task GuardarAsync()
    {
        await _context.SaveChangesAsync();
    }
}
