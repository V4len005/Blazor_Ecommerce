using Blazor_Ecommerce.Models;

namespace Blazor_Ecommerce.Services;

/// <summary>Consultas de resumen que alimentan el panel de administración.</summary>
public interface IEstadisticaService
{
    /// <summary>
    /// Calcula las estadísticas del panel con consultas a la base de datos: cantidad de productos,
    /// productos con stock bajo, pedidos de hoy, total vendido, productos más vendidos y pedidos por estado.
    /// Los pedidos cancelados no cuentan como ventas.
    /// </summary>
    /// <returns>Los datos listos para mostrar.</returns>
    Task<EstadisticasPanel> ObtenerPanel();
}