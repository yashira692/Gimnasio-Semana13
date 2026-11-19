namespace Gimnasio.Negocio.Seguridad;

// Roles propios del gimnasio
public static class Roles
{
    public const string Recepcionista = "Recepcionista";
    public const string Gerente = "Gerente";

    // Rol que el servidor asigna a toda cuenta nueva
    public const string PorDefecto = Recepcionista;
}
