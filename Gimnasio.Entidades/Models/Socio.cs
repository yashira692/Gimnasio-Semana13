using System;
using System.Collections.Generic;

namespace Gimnasio.Entidades.Models;

public partial class Socio
{
    public int SocioId { get; set; }

    public string Nombres { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string Dni { get; set; } = null!;

    public string? Email { get; set; }

    public DateOnly FechaInscripcion { get; set; }

    public bool Activo { get; set; }

    public int PlanId { get; set; }

    public virtual Plan Plan { get; set; } = null!;
}
