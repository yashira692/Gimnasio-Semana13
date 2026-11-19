using System.ComponentModel.DataAnnotations;

namespace Gimnasio.Entidades.DTOs;

// Login: correo y clave
public class LoginDto
{
    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La clave es obligatoria")]
    public string Clave { get; set; } = string.Empty;
}

// Registro: correo y clave (SIN rol: el rol lo asigna el servidor)
public class RegistroDto
{
    [Required(ErrorMessage = "El correo es obligatorio")]
    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido")]
    [StringLength(100)]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La clave es obligatoria")]
    [StringLength(50, MinimumLength = 8, ErrorMessage = "La clave debe tener entre 8 y 50 caracteres")]
    public string Clave { get; set; } = string.Empty;
}

// Respuesta del login: token y fecha de expiración (UTC)
public class TokenDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expira { get; set; }
}

// Perfil del usuario autenticado (sale de los claims del token)
public class PerfilDto
{
    public string Correo { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
}
