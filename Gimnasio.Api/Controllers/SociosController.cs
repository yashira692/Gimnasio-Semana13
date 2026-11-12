using Gimnasio.Entidades.DTOs;
using Gimnasio.Negocio.Excepciones;
using Gimnasio.Negocio.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace Gimnasio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SociosController : ControllerBase
{
    private readonly ISocioService _service;

    public SociosController(ISocioService service)
    {
        _service = service;
    }

    // GET api/socios?planId=2&nombre=ram&activo=true  (filtros por query string)
    [HttpGet]
    public async Task<ActionResult<List<SocioDto>>> Listar(
        [FromQuery] int? planId, [FromQuery] string? nombre, [FromQuery] bool? activo)
    {
        return Ok(await _service.ListarAsync(planId, nombre, activo));
    }

    // GET api/socios/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<SocioDto>> Obtener(int id)
    {
        var socio = await _service.ObtenerAsync(id);
        if (socio is null) return NotFound(new { mensaje = $"No existe el socio con id {id}." });
        return Ok(socio);
    }

    // POST api/socios
    [HttpPost]
    public async Task<ActionResult<SocioDto>> Crear(SocioCrearDto dto)
    {
        try
        {
            var creado = await _service.CrearAsync(dto);
            return CreatedAtAction(nameof(Obtener), new { id = creado.SocioId }, creado);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    // PUT api/socios/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, SocioActualizarDto dto)
    {
        try
        {
            var ok = await _service.ActualizarAsync(id, dto);
            if (!ok) return NotFound(new { mensaje = $"No existe el socio con id {id}." });
            return NoContent();
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    // DELETE api/socios/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var ok = await _service.EliminarAsync(id);
        if (!ok) return NotFound(new { mensaje = $"No existe el socio con id {id}." });
        return NoContent();
    }
}
