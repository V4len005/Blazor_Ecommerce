namespace Blazor_Ecommerce.Services;

/// <summary>
/// Identifica el arranque actual de la aplicación. Se agrega a las direcciones de los archivos
/// estáticos para que, después de cada despliegue, el navegador descargue los nuevos
/// en lugar de usar copias viejas guardadas.
/// </summary>
public static class VersionApp
{
    /// <summary>Cambia cada vez que la aplicación arranca (es decir, en cada despliegue).</summary>
    public static readonly string Valor = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
}