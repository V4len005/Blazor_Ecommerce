namespace Blazor_Ecommerce.Models;

/// <summary>Reglas de transición entre los estados de un pedido.</summary>
public static class EstadoPedidoExtensiones
{
    /// <summary>Estados a los que puede pasar un pedido desde el estado actual.</summary>
    /// <param name="actual">Estado actual del pedido.</param>
    /// <returns>Los estados permitidos; vacío si el pedido ya no admite cambios.</returns>
    public static IReadOnlyList<EstadoPedido> Siguientes(this EstadoPedido actual) => actual switch
    {
        EstadoPedido.Confirmado => new[] { EstadoPedido.Enviado, EstadoPedido.Cancelado },
        EstadoPedido.Enviado => new[] { EstadoPedido.Entregado, EstadoPedido.Cancelado },
        _ => Array.Empty<EstadoPedido>()
    };

    /// <summary>Indica si un pedido puede pasar del estado actual a otro.</summary>
    /// <param name="actual">Estado actual del pedido.</param>
    /// <param name="nuevo">Estado al que se quiere pasar.</param>
    /// <returns>True si la transición está permitida.</returns>
    public static bool PuedePasarA(this EstadoPedido actual, EstadoPedido nuevo) =>
        actual.Siguientes().Contains(nuevo);
}