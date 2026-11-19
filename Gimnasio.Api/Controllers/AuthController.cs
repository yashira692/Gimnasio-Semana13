using Gimnasio.Entidades.DTOs;
using Gimnasio.Negocio.Excepciones;
using Gimnasio.Negocio.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gimnasio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    // POST api/auth/registro  (público). La cuenta nace con rol Recepcionista.
    [HttpPost("registro")]
    [AllowAnonymous]
    public async Task<ActionResult<PerfilDto>> Registro(RegistroDto dto)
    {
        try
        {
            var perfil = await _service.RegistrarAsync(dto);
            return StatusCode(StatusCodes.Status201Created, perfil);
        }
        catch (ReglaNegocioException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    // POST api/auth/login  (público). 401 si el correo no existe o la clave es incorrecta.
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<TokenDto>> Login(LoginDto dto)
    {
        var token = await _service.LoginAsync(dto);
        if (token is null) return Unauthorized(new { mensaje = "Correo o clave incorrectos." });
        return Ok(token);
    }

    // GET api/auth/perfil  (solo autenticados). Datos tomados de los claims del token.
    [HttpGet("perfil")]
    [Authorize]
    public ActionResult<PerfilDto> Perfil()
    {
        var perfil = new PerfilDto
        {
            Correo = User.Claims.FirstOrDefault(c => c.Type == "email")?.Value ?? string.Empty,
            Rol = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? string.Empty
        };
        return Ok(perfil);
    }
}
