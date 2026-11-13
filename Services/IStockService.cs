using Blazor_Ecommerce.Models;

namespace Blazor_Ecommerce.Services;

/// <summary>Operaciones manuales sobre el stock de los productos y consulta de su historial.</summary>
public interface IStockService
{
    /// <summary>Registra una entrada de mercadería por compra.</summary>
    /// <param name="productoId">Producto que recibe unidades.</param>
    /// <param name="cantidad">Unidades recibidas (mayor a 0).</param>
    /// <param name="observacion">Detalle opcional (hasta 200 caracteres).</param>
    /// <returns>El movimiento registrado.</returns>
    /// <exception cref="InvalidOperationException">Si el producto no existe, los datos no son válidos o el stock cambió mientras se operaba.</exception>
    Task<MovimientoStock> RegistrarEntrada(int productoId, int cantidad, string? observacion);

    /// <summary>Registra una salida por merma (pérdida, rotura, vencimiento).</summary>
    /// <param name="productoId">Producto que pierde unidades.</param>
    /// <param name="cantidad">Unidades perdidas (mayor a 0 y no mayores al stock actual).</param>
    /// <param name="observacion">Detalle opcional (hasta 200 caracteres).</param>
    /// <returns>El movimiento registrado.</returns>
    /// <exception cref="InvalidOperationException">Si no hay stock suficiente, el producto no existe o el stock cambió mientras se operaba.</exception>
    Task<MovimientoStock> RegistrarMerma(int productoId, int cantidad, string? observacion);

    /// <summary>Lleva el stock a la cantidad realmente contada, registrando la diferencia.</summary>
    /// <param name="productoId">Producto a ajustar.</param>
    /// <param name="stockReal">Cantidad contada físicamente (0 o más).</param>
    /// <param name="observacion">Detalle opcional (hasta 200 caracteres).</param>
    /// <returns>El movimiento de ajuste registrado.</returns>
    /// <exception cref="InvalidOperationException">Si el stock real coincide con el actual, el producto no existe o el stock cambió mientras se operaba.</exception>
    Task<MovimientoStock> AjustarStock(int productoId, int stockReal, string? observacion);

    /// <summary>Obtiene una página del historial de movimientos, del más reciente al más antiguo.</summary>
    /// <param name="productoId">Si tiene valor, solo movimientos de ese producto.</param>
    /// <param name="tipo">Si tiene valor, solo movimientos de ese tipo.</param>
    /// <param name="motivo">Si tiene valor, solo movimientos con ese motivo.</param>
    /// <param name="pagina">Número de página pedido (empieza en 1; se ajusta si queda fuera de rango).</param>
    /// <param name="tamanioPagina">Cantidad de movimientos por página.</param>
    /// <returns>La página de movimientos con su producto cargado.</returns>
    Task<ResultadoPaginado<MovimientoStock>> ObtenerHistorial(
        int? productoId, TipoMovimiento? tipo, MotivoMovimiento? motivo, int pagina, int tamanioPagina);
}