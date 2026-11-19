using Gimnasio.Entidades.DTOs;

namespace Gimnasio.Negocio.Servicios;

public interface IAuthService
{
    Task<PerfilDto> RegistrarAsync(RegistroDto dto);
    Task<TokenDto?> LoginAsync(LoginDto dto);
}
