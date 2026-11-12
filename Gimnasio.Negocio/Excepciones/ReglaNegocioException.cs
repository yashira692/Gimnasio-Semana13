namespace Gimnasio.Negocio.Excepciones;

// Excepción propia: se lanza cuando se incumple una regla de negocio.
// El controlador la convierte en 409 Conflict.
public class ReglaNegocioException : Exception
{
    public ReglaNegocioException(string mensaje) : base(mensaje) { }
}
