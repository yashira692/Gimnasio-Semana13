using Gimnasio.Entidades.DTOs;
using Gimnasio.Negocio.Excepciones;
using Gimnasio.Negocio.Seguridad;
using Gimnasio.Negocio.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gimnasio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // por defecto: solo autenticados
public class PlanesController : ControllerBase
{
    private readonly IPlanService _service;

    public PlanesController(IPlanService service)
    {
        _service = service;
    }

    // GET api/planes  (público)
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<PlanDto>>> Listar()
    {
        return Ok(await _service.ListarAsync());
    }

    // GET api/planes/5  (público)
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<PlanDto>> Obtener(int id)
    {
        var plan = await _service.ObtenerAsync(id);
        if (plan is null) return NotFound(new { mensaje = $"No existe el plan con id {id}." });
        return Ok(plan);
    }

    // GET api/planes/5/socios  (ruta anidada, solo autenticados)
    [HttpGet("{id:int}/socios")]
    public async Task<ActionResult<List<SocioDto>>> ListarSocios(int id)
    {
        var socios = await _service.ListarSociosDelPlanAsync(id);
        if (socios is null) return NotFound(new { mensaje = $"No existe el plan con id {id}." });
        return Ok(socios);
    }

    // POST api/planes  (solo Gerente)
    [HttpPost]
    [Authorize(Roles = Roles.Gerente)]
    public async Task<ActionResult<PlanDto>> Crear(PlanCrearDto dto)
    {
        try
        {
            var creado = await _service.CrearAsync(dto);
            return CreatedAtAction(nameof(Obtener), new { id = creado.PlanId }, creado);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    // PUT api/planes/5  (solo Gerente)
    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Gerente)]
    public async Task<IActionResult> Actualizar(int id, PlanActualizarDto dto)
    {
        try
        {
            var ok = await _service.ActualizarAsync(id, dto);
            if (!ok) return NotFound(new { mensaje = $"No existe el plan con id {id}." });
            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    // DELETE api/planes/5  (solo Gerente)
    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Gerente)]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            var ok = await _service.EliminarAsync(id);
            if (!ok) return NotFound(new { mensaje = $"No existe el plan con id {id}." });
            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }
}
