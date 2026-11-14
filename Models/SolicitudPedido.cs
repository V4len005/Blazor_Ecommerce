namespace Blazor_Ecommerce.Models;

/// <summary>Una línea del carrito: solo el producto y la cantidad. Nunca incluye precios.</summary>
/// <param name="ProductoId">Producto elegido.</param>
/// <param name="Cantidad">Unidades pedidas.</param>
public record LineaPedido(int ProductoId, int Cantidad);

/// <summary>Datos que la pantalla de checkout entrega al servicio para confirmar un pedido.</summary>
/// <param name="NombreCliente">Nombre de quien compra.</param>
/// <param name="Email">Correo de contacto.</param>
/// <param name="Telefono">Teléfono (opcional).</param>
/// <param name="DireccionEnvio">Dirección de entrega; nula si retira en el local.</param>
/// <param name="Observaciones">Comentarios del cliente (opcional).</param>
/// <param name="Lineas">Productos y cantidades del carrito.</param>
public record SolicitudPedido(
    string NombreCliente,
    string Email,
    string? Telefono,
    string? DireccionEnvio,
    string? Observaciones,
    IReadOnlyList<LineaPedido> Lineas);