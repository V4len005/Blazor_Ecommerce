using Blazor_Ecommerce.Data;
using Blazor_Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Blazor_Ecommerce.Services;

/// <summary>Implementación de <see cref="IStockService"/> con EF Core.</summary>
public class StockService : IStockService
{
    private const int LargoMaximoObservacion = 200;

    private readonly IDbContextFactory<TiendaDbContext> _fabrica;

    public StockService(IDbContextFactory<TiendaDbContext> fabrica)
    {
        _fabrica = fabrica;
    }

    public Task<MovimientoStock> RegistrarEntrada(int productoId, int cantidad, string? observacion) =>
        Registrar(productoId, TipoMovimiento.Entrada, MotivoMovimiento.Compra, cantidad, observacion);

    public Task<MovimientoStock> RegistrarMerma(int productoId, int cantidad, string? observacion) =>
        Registrar(productoId, TipoMovimiento.Salida, MotivoMovimiento.Merma, cantidad, observacion);

    public async Task<MovimientoStock> AjustarStock(int productoId, int stockReal, string? observacion)
    {
        if (stockReal < 0)
        {
            throw new InvalidOperationException("El stock real no puede ser negativo.");
        }

        var texto = LimpiarObservacion(observacion);

        await using var contexto = await _fabrica.CreateDbContextAsync();

        var producto = await BuscarProducto(contexto, productoId);
        var diferencia = stockReal - producto.StockActual;

        if (diferencia == 0)
        {
            throw new InvalidOperationException("El stock real coincide con el actual: no hay nada que ajustar.");
        }

        var movimiento = AplicadorStock.Aplicar(
            contexto, producto, TipoMovimiento.Ajuste, MotivoMovimiento.Ajuste, diferencia, texto);

        await Guardar(contexto);

        return movimiento;
    }

    public async Task<ResultadoPaginado<MovimientoStock>> ObtenerHistorial(
        int? productoId, TipoMovimiento? tipo, MotivoMovimiento? motivo, int pagina, int tamanioPagina)
    {
        await using var contexto = await _fabrica.CreateDbContextAsync();

        IQueryable<MovimientoStock> consulta = contexto.MovimientosStock.AsNoTracking();

        if (productoId.HasValue)
        {
            consulta = consulta.Where(m => m.ProductoId == productoId.Value);
        }

        if (tipo.HasValue)
        {
            consulta = consulta.Where(m => m.Tipo == tipo.Value);
        }

        if (motivo.HasValue)
        {
            consulta = consulta.Where(m => m.Motivo == motivo.Value);
        }

        var total = await consulta.CountAsync();

        var tamanio = Math.Max(1, tamanioPagina);
        var totalPaginas = Math.Max(1, (int)Math.Ceiling(total / (double)tamanio));
        var paginaValida = Math.Clamp(pagina, 1, totalPaginas);

        var items = await consulta
            .Include(m => m.Producto)
            .OrderByDescending(m => m.Fecha)
            .ThenByDescending(m => m.Id)
            .Skip((paginaValida - 1) * tamanio)
            .Take(tamanio)
            .ToListAsync();

        return new ResultadoPaginado<MovimientoStock>
        {
            Items = items,
            TotalRegistros = total,
            Pagina = paginaValida,
            TamanioPagina = tamanio
        };
    }

    private async Task<MovimientoStock> Registrar(
        int productoId, TipoMovimiento tipo, MotivoMovimiento motivo, int cantidad, string? observacion)
    {
        var texto = LimpiarObservacion(observacion);

        await using var contexto = await _fabrica.CreateDbContextAsync();

        var producto = await BuscarProducto(contexto, productoId);

        var movimiento = AplicadorStock.Aplicar(contexto, producto, tipo, motivo, cantidad, texto);

        await Guardar(contexto);

        return movimiento;
    }

    // Busca el producto con seguimiento activo, porque su stock va a cambiar.
    private static async Task<Producto> BuscarProducto(TiendaDbContext contexto, int productoId)
    {
        var producto = await contexto.Productos.FirstOrDefaultAsync(p => p.Id == productoId);

        return producto ?? throw new InvalidOperationException("El producto ya no existe.");
    }

    private static string? LimpiarObservacion(string? observacion)
    {
        var texto = string.IsNullOrWhiteSpace(observacion) ? null : observacion.Trim();

        if (texto is not null && texto.Length > LargoMaximoObservacion)
        {
            throw new InvalidOperationException(
                $"La observación no puede superar los {LargoMaximoObservacion} caracteres.");
        }

        return texto;
    }

    private static async Task Guardar(TiendaDbContext contexto)
    {
        try
        {
            // Guarda el nuevo stock y el movimiento en una sola transacción.
            await contexto.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new InvalidOperationException(
                "El stock cambió mientras operabas. Actualizá la pantalla y volvé a intentarlo.", ex);
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException("No se pudo registrar el movimiento de stock.", ex);
        }
    }
}