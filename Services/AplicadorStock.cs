using Blazor_Ecommerce.Data;
using Blazor_Ecommerce.Models;

namespace Blazor_Ecommerce.Services;

/// <summary>
/// Único punto donde se modifica el stock de un producto. Cambia el stock y agrega el
/// movimiento al contexto, pero NO guarda: quien lo llama decide cuándo hacer SaveChanges,
/// así el stock puede cambiar en la misma transacción que un pedido.
/// </summary>
public static class AplicadorStock
{
    /// <summary>Aplica un movimiento al producto y lo agrega al contexto sin guardar.</summary>
    /// <param name="contexto">Contexto en el que está cargado el producto.</param>
    /// <param name="producto">Producto cuyo stock cambia (debe estar rastreado por el contexto).</param>
    /// <param name="tipo">Entrada suma, Salida resta, Ajuste suma la cantidad con su signo.</param>
    /// <param name="motivo">Razón del cambio.</param>
    /// <param name="cantidad">Unidades. Positiva en Entrada y Salida; con signo, y distinta de 0, en Ajuste.</param>
    /// <param name="observacion">Detalle opcional.</param>
    /// <returns>El movimiento agregado al contexto (todavía sin guardar).</returns>
    /// <exception cref="InvalidOperationException">Si la cantidad no es válida o el stock quedaría negativo.</exception>
    public static MovimientoStock Aplicar(
        TiendaDbContext contexto,
        Producto producto,
        TipoMovimiento tipo,
        MotivoMovimiento motivo,
        int cantidad,
        string? observacion)
    {
        if (tipo == TipoMovimiento.Ajuste ? cantidad == 0 : cantidad <= 0)
        {
            throw new InvalidOperationException(tipo == TipoMovimiento.Ajuste
                ? "El ajuste no puede ser 0."
                : "La cantidad debe ser mayor a 0.");
        }

        var variacion = tipo == TipoMovimiento.Salida ? -cantidad : cantidad;
        var nuevoStock = producto.StockActual + variacion;

        if (nuevoStock < 0)
        {
            throw new InvalidOperationException(
                $"Stock insuficiente de \"{producto.Nombre}\": hay {producto.StockActual} unidad(es) y el movimiento pide {Math.Abs(variacion)}.");
        }

        producto.StockActual = nuevoStock;

        var movimiento = new MovimientoStock
        {
            ProductoId = producto.Id,
            Tipo = tipo,
            Motivo = motivo,
            Cantidad = cantidad,
            Fecha = DateTime.UtcNow,
            Observacion = observacion
        };

        contexto.MovimientosStock.Add(movimiento);

        return movimiento;
    }
}