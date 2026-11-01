using Blazor_Ecommerce.Models;

namespace Blazor_Ecommerce.Data;

/// <summary>
/// Movimientos iniciales: uno por cada producto de ejemplo con stock, para que
/// el historial coincida con el stock actual. El producto 4 no tiene (su stock es 0).
/// </summary>
public static class SemillaMovimientos
{
    private static readonly DateTime FechaInicial = new(2026, 10, 1, 0, 0, 0, DateTimeKind.Utc);

    public static readonly MovimientoStock[] Movimientos =
    [
        StockInicial(1, 1, 15),
        StockInicial(2, 2, 8),
        StockInicial(3, 3, 10),
        StockInicial(4, 5, 25),
        StockInicial(5, 6, 40),
        StockInicial(6, 7, 2),
        StockInicial(7, 8, 18),
        StockInicial(8, 9, 12),
        StockInicial(9, 10, 6),
        StockInicial(10, 11, 30),
        StockInicial(11, 12, 20)
    ];

    private static MovimientoStock StockInicial(int id, int productoId, int cantidad) => new()
    {
        Id = id,
        ProductoId = productoId,
        Tipo = TipoMovimiento.Ajuste,
        Motivo = MotivoMovimiento.Ajuste,
        Cantidad = cantidad,
        Fecha = FechaInicial,
        Observacion = "Stock inicial"
    };
}