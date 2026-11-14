namespace Blazor_Ecommerce.Models;

/// <summary>Producto con la cantidad de unidades vendidas.</summary>
/// <param name="ProductoId">Identificador del producto.</param>
/// <param name="Nombre">Nombre del producto.</param>
/// <param name="Unidades">Unidades vendidas en pedidos no cancelados.</param>
public record ProductoVendido(int ProductoId, string Nombre, int Unidades);

/// <summary>Producto activo cuyo stock está en su mínimo o por debajo.</summary>
/// <param name="ProductoId">Identificador del producto.</param>
/// <param name="Nombre">Nombre del producto.</param>
/// <param name="StockActual">Unidades disponibles.</param>
/// <param name="StockMinimo">Umbral de aviso.</param>
public record ProductoStockBajo(int ProductoId, string Nombre, int StockActual, int StockMinimo);

/// <summary>Cantidad de pedidos en un estado.</summary>
/// <param name="Estado">Estado del pedido.</param>
/// <param name="Cantidad">Cantidad de pedidos en ese estado.</param>
public record ConteoEstado(EstadoPedido Estado, int Cantidad);

/// <summary>Datos que muestra el panel de administración.</summary>
/// <param name="ProductosTotales">Todos los productos, activos o no.</param>
/// <param name="ProductosActivos">Productos visibles en la tienda.</param>
/// <param name="ProductosStockBajo">Productos activos con stock igual o menor a su mínimo (incluye agotados).</param>
/// <param name="PedidosHoy">Pedidos no cancelados realizados hoy (hora de Argentina).</param>
/// <param name="TotalVendido">Suma de los totales de los pedidos no cancelados.</param>
/// <param name="PedidosPorEstado">Cantidad de pedidos de cada estado (incluye los estados en cero).</param>
/// <param name="MasVendidos">Los productos más vendidos, del primero al último.</param>
/// <param name="StockBajo">Los productos con stock bajo más urgentes (hasta 5).</param>
public record EstadisticasPanel(
    int ProductosTotales,
    int ProductosActivos,
    int ProductosStockBajo,
    int PedidosHoy,
    decimal TotalVendido,
    IReadOnlyList<ConteoEstado> PedidosPorEstado,
    IReadOnlyList<ProductoVendido> MasVendidos,
    IReadOnlyList<ProductoStockBajo> StockBajo);