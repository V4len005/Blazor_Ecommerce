using Blazor_Ecommerce.Data;
using Blazor_Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Blazor_Ecommerce.Services;

/// <summary>Implementación de <see cref="IEstadisticaService"/> con EF Core.</summary>
public class EstadisticaService : IEstadisticaService
{
    private const int CantidadMasVendidos = 5;
    private const int CantidadStockBajo = 5;

    private readonly IDbContextFactory<TiendaDbContext> _fabrica;
    private readonly ILogger<EstadisticaService> _logger;

    public EstadisticaService(IDbContextFactory<TiendaDbContext> fabrica, ILogger<EstadisticaService> logger)
    {
        _fabrica = fabrica;
        _logger = logger;
    }

    public async Task<EstadisticasPanel> ObtenerPanel()
    {
        await using var contexto = await _fabrica.CreateDbContextAsync();

        // Consultas base, todavía sin ejecutar: cada resultado les agrega su propia condición.
        var productos = contexto.Productos.AsNoTracking();
        var pedidosValidos = contexto.Pedidos.AsNoTracking()
            .Where(p => p.Estado != EstadoPedido.Cancelado);
        var productosStockBajo = productos
            .Where(p => p.Activo && p.StockActual <= p.StockMinimo);

        var productosTotales = await productos.CountAsync();
        var productosActivos = await productos.CountAsync(p => p.Activo);
        var cantidadStockBajo = await productosStockBajo.CountAsync();

        var stockBajo = await productosStockBajo
            .OrderBy(p => p.StockActual)
            .ThenBy(p => p.Nombre)
            .Take(CantidadStockBajo)
            .Select(p => new ProductoStockBajo(p.Id, p.Nombre, p.StockActual, p.StockMinimo))
            .ToListAsync();

        // Medianoche de Argentina expresada en UTC, porque las fechas se guardan en UTC.
        var inicioDeHoy = Formato.InicioDelDiaUtc();
        var pedidosHoy = await pedidosValidos.CountAsync(p => p.Fecha >= inicioDeHoy);

        // Sobre una tabla vacía devuelve 0 (EF lo traduce con COALESCE).
        var totalVendido = await pedidosValidos.SumAsync(p => p.Total);

        // GROUP BY Estado: cuántos pedidos hay en cada estado.
        var porEstado = await contexto.Pedidos.AsNoTracking()
            .GroupBy(p => p.Estado)
            .Select(g => new { Estado = g.Key, Cantidad = g.Count() })
            .ToListAsync();

        // GROUP BY producto: unidades vendidas en pedidos no cancelados.
        var vendidos = await contexto.PedidoItems.AsNoTracking()
            .Where(i => i.Pedido!.Estado != EstadoPedido.Cancelado)
            .GroupBy(i => new { i.ProductoId, i.Producto!.Nombre })
            .Select(g => new { g.Key.ProductoId, g.Key.Nombre, Unidades = g.Sum(i => i.Cantidad) })
            .OrderByDescending(v => v.Unidades)
            .ThenBy(v => v.Nombre)
            .Take(CantidadMasVendidos)
            .ToListAsync();

        // Todos los estados aparecen, aunque no tengan pedidos (con 0).
        var conteos = Enum.GetValues<EstadoPedido>()
            .Select(estado => new ConteoEstado(
                estado, porEstado.FirstOrDefault(x => x.Estado == estado)?.Cantidad ?? 0))
            .ToList();

        var masVendidos = vendidos
            .Select(v => new ProductoVendido(v.ProductoId, v.Nombre, v.Unidades))
            .ToList();

        _logger.LogDebug(
            "Estadísticas del panel calculadas: {Productos} productos activos, {PedidosHoy} pedido(s) de hoy.",
            productosActivos, pedidosHoy);

        return new EstadisticasPanel(
            productosTotales,
            productosActivos,
            cantidadStockBajo,
            pedidosHoy,
            totalVendido,
            conteos,
            masVendidos,
            stockBajo);
    }
}