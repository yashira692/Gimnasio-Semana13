using System.ComponentModel.DataAnnotations;

namespace Gimnasio.Entidades.DTOs;

// DTO de lectura: devuelve el nombre del plan en lugar del objeto Plan
public class SocioDto
{
    public int SocioId { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string Dni { get; set; } = string.Empty;
    public string? Email { get; set; }
    public DateOnly FechaInscripcion { get; set; }
    public bool Activo { get; set; }
    public int PlanId { get; set; }
    public string Plan { get; set; } = string.Empty;
}

// DTO de entrada para crear
public class SocioCrearDto
{
    [Required(ErrorMessage = "Los nombres son obligatorios")]
    [StringLength(60, MinimumLength = 2, ErrorMessage = "Los nombres deben tener entre 2 y 60 caracteres")]
    public string Nombres { get; set; } = string.Empty;

    [Required(ErrorMessage = "Los apellidos son obligatorios")]
    [StringLength(60, MinimumLength = 2, ErrorMessage = "Los apellidos deben tener entre 2 y 60 caracteres")]
    public string Apellidos { get; set; } = string.Empty;

    [Required(ErrorMessage = "El DNI es obligatorio")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener 8 dígitos")]
    [RegularExpression("^[0-9]{8}$", ErrorMessage = "El DNI solo debe contener números")]
    public string Dni { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe indicar un plan válido")]
    public int PlanId { get; set; }
}

// DTO de entrada para actualizar
public class SocioActualizarDto
{
    [Required(ErrorMessage = "Los nombres son obligatorios")]
    [StringLength(60, MinimumLength = 2, ErrorMessage = "Los nombres deben tener entre 2 y 60 caracteres")]
    public string Nombres { get; set; } = string.Empty;

    [Required(ErrorMessage = "Los apellidos son obligatorios")]
    [StringLength(60, MinimumLength = 2, ErrorMessage = "Los apellidos deben tener entre 2 y 60 caracteres")]
    public string Apellidos { get; set; } = string.Empty;

    [Required(ErrorMessage = "El DNI es obligatorio")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "El DNI debe tener 8 dígitos")]
    [RegularExpression("^[0-9]{8}$", ErrorMessage = "El DNI solo debe contener números")]
    public string Dni { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe indicar un plan válido")]
    public int PlanId { get; set; }

    [Required]
    public bool Activo { get; set; }
}
