using Blazor_Ecommerce.Models;

namespace Blazor_Ecommerce.Services;

/// <summary>Operaciones sobre los pedidos de la tienda.</summary>
public interface IPedidoService
{
    /// <summary>
    /// Confirma un pedido: en una sola transacción crea el pedido y sus ítems, descuenta el
    /// stock y registra un movimiento de stock por cada producto. Los precios y el total
    /// se toman de la base de datos, nunca del carrito.
    /// </summary>
    /// <param name="solicitud">Datos del cliente y líneas del carrito (producto y cantidad).</param>
    /// <returns>El pedido confirmado, con su Id asignado.</returns>
    /// <exception cref="InvalidOperationException">
    /// Si el carrito está vacío, un producto ya no está disponible, no hay stock suficiente,
    /// los datos del cliente no son válidos o el stock cambió mientras se confirmaba.
    /// </exception>
    Task<Pedido> ConfirmarPedido(SolicitudPedido solicitud);

    /// <summary>Busca un pedido por su identificador, con sus ítems y los productos de cada ítem.</summary>
    /// <param name="id">Identificador del pedido.</param>
    /// <returns>El pedido, o null si no existe.</returns>
    Task<Pedido?> ObtenerPorId(int id);

    /// <summary>Obtiene una página de pedidos, del más reciente al más antiguo, con filtros opcionales.</summary>
    /// <param name="estado">Si tiene valor, solo pedidos en ese estado.</param>
    /// <param name="busqueda">Texto a buscar en el nombre o email del cliente, o un número de pedido (puede ser nulo).</param>
    /// <param name="pagina">Número de página pedido (empieza en 1; se ajusta si queda fuera de rango).</param>
    /// <param name="tamanioPagina">Cantidad de pedidos por página.</param>
    /// <returns>La página de pedidos con sus ítems cargados y los totales.</returns>
    Task<ResultadoPaginado<Pedido>> ObtenerPaginado(
        EstadoPedido? estado, string? busqueda, int pagina, int tamanioPagina);

    /// <summary>
    /// Cambia el estado de un pedido respetando las transiciones permitidas. Si el nuevo estado
    /// es Cancelado, devuelve el stock de cada producto con un movimiento de Entrada por
    /// Cancelación, en la misma transacción que el cambio de estado.
    /// </summary>
    /// <param name="pedidoId">Pedido a modificar.</param>
    /// <param name="nuevoEstado">Estado al que debe pasar.</param>
    /// <returns>El pedido con su nuevo estado.</returns>
    /// <exception cref="InvalidOperationException">
    /// Si el pedido no existe, la transición no está permitida o el pedido
    /// (o su stock) cambió mientras se operaba.
    /// </exception>
    Task<Pedido> CambiarEstado(int pedidoId, EstadoPedido nuevoEstado);
}