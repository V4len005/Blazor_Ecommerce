namespace Blazor_Ecommerce.Models;

/// <summary>Cómo recibe el cliente su pedido.</summary>
public enum TipoEntrega
{
    /// <summary>El cliente retira el pedido en el local.</summary>
    Retiro,

    /// <summary>El pedido se envía a la dirección del cliente.</summary>
    Domicilio
}