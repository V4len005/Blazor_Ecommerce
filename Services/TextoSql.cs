namespace Blazor_Ecommerce.Services;

/// <summary>Utilidades para armar consultas de texto seguras contra SQL Server.</summary>
public static class TextoSql
{
    /// <summary>
    /// Escapa los caracteres que tienen significado especial en un LIKE ([, % y _),
    /// para que se busquen como texto común.
    /// </summary>
    /// <param name="texto">Texto ingresado por la persona.</param>
    /// <returns>El texto listo para usarse dentro de un patrón LIKE.</returns>
    public static string EscaparLike(string texto) =>
        texto.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");
}