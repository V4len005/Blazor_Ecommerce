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
}