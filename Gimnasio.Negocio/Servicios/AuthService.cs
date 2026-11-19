using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Gimnasio.Datos.Repositorios;
using Gimnasio.Entidades.DTOs;
using Gimnasio.Entidades.Models;
using Gimnasio.Negocio.Excepciones;
using Gimnasio.Negocio.Seguridad;
using Microsoft.IdentityModel.Tokens;

namespace Gimnasio.Negocio.Servicios;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _repo;
    private readonly IMapper _mapper;
    private readonly JwtOpciones _jwt;

    public AuthService(IUsuarioRepository repo, IMapper mapper, JwtOpciones jwt)
    {
        _repo = repo;
        _mapper = mapper;
        _jwt = jwt;
    }

    public async Task<PerfilDto> RegistrarAsync(RegistroDto dto)
    {
        var correo = dto.Correo.Trim().ToLowerInvariant();

        // Regla: el correo es único
        if (await _repo.BuscarPorCorreoAsync(correo) is not null)
            throw new ReglaNegocioException($"Ya existe una cuenta con el correo {correo}.");

        var usuario = new Usuario
        {
            Correo = correo,
            ClaveHash = BCrypt.Net.BCrypt.HashPassword(dto.Clave), // hash con salt (work factor 11)
            Rol = Roles.PorDefecto,                                 // el rol lo decide el servidor
            FechaRegistro = DateTime.UtcNow
        };

        await _repo.AgregarAsync(usuario);
        await _repo.GuardarAsync();

        return _mapper.Map<PerfilDto>(usuario);
    }

    public async Task<TokenDto?> LoginAsync(LoginDto dto)
    {
        var correo = dto.Correo.Trim().ToLowerInvariant();
        var usuario = await _repo.BuscarPorCorreoAsync(correo);

        // Mismo resultado (null -> 401) si el correo no existe o si la clave es incorrecta
        if (usuario is null || !BCrypt.Net.BCrypt.Verify(dto.Clave, usuario.ClaveHash))
            return null;

        return GenerarToken(usuario);
    }

    private TokenDto GenerarToken(Usuario usuario)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuario.UsuarioId.ToString()),
            new(JwtRegisteredClaimNames.Email, usuario.Correo),
            new("role", usuario.Rol),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Clave));
        var credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
        var ahora = DateTime.UtcNow;
        var expira = ahora.AddMinutes(_jwt.MinutosExpiracion);

        var token = new JwtSecurityToken(
            issuer: _jwt.Emisor,
            audience: _jwt.Audiencia,
            claims: claims,
            notBefore: ahora,
            expires: expira,
            signingCredentials: credenciales);

        return new TokenDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expira = expira
        };
    }
}
