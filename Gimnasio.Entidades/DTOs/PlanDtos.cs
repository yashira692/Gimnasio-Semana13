using System.ComponentModel.DataAnnotations;

namespace Gimnasio.Entidades.DTOs;

// DTO de lectura: lo que la API devuelve
public class PlanDto
{
    public int PlanId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal PrecioMensual { get; set; }
    public int DuracionMeses { get; set; }
    public bool Activo { get; set; }
    public int CantidadSocios { get; set; }
}

// DTO de entrada para crear
public class PlanCrearDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "La descripción no puede superar 200 caracteres")]
    public string? Descripcion { get; set; }

    [Range(1, 1000, ErrorMessage = "El precio mensual debe estar entre 1 y 1000")]
    public decimal PrecioMensual { get; set; }

    [Range(1, 24, ErrorMessage = "La duración debe estar entre 1 y 24 meses")]
    public int DuracionMeses { get; set; }
}

// DTO de entrada para actualizar
public class PlanActualizarDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "La descripción no puede superar 200 caracteres")]
    public string? Descripcion { get; set; }

    [Range(1, 1000, ErrorMessage = "El precio mensual debe estar entre 1 y 1000")]
    public decimal PrecioMensual { get; set; }

    [Range(1, 24, ErrorMessage = "La duración debe estar entre 1 y 24 meses")]
    public int DuracionMeses { get; set; }

    [Required]
    public bool Activo { get; set; }
}
