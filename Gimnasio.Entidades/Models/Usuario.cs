namespace Gimnasio.Entidades.Models;

// Usuario del sistema (personal del gimnasio). Tabla: Usuario
public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string Correo { get; set; } = null!;

    // Nunca se guarda la contraseña en texto: solo su hash BCrypt
    public string ClaveHash { get; set; } = null!;

    // Recepcionista o Gerente (lo decide el servidor)
    public string Rol { get; set; } = null!;

    public DateTime FechaRegistro { get; set; }
}
