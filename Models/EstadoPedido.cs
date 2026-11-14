namespace Blazor_Ecommerce.Models;

/// <summary>Etapas por las que pasa un pedido.</summary>
public enum EstadoPedido
{
    /// <summary>El pedido fue confirmado y el stock ya se descontó.</summary>
    Confirmado,

    /// <summary>El pedido salió hacia el cliente.</summary>
    Enviado,

    /// <summary>El cliente recibió el pedido.</summary>
    Entregado,

    /// <summary>El pedido se canceló y el stock se devolvió.</summary>
    Cancelado
}