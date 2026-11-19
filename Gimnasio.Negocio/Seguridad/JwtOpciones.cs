namespace Gimnasio.Negocio.Seguridad;

// Se llena desde la sección "Jwt" de la configuración.
// La Clave NO está en appsettings.json: va en User Secrets (dotnet user-secrets).
public class JwtOpciones
{
    public string Clave { get; set; } = string.Empty;
    public string Emisor { get; set; } = string.Empty;
    public string Audiencia { get; set; } = string.Empty;
    public int MinutosExpiracion { get; set; } = 60;
}
