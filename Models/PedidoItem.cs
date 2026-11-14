using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazor_Ecommerce.Models;

/// <summary>Una línea de un pedido: qué producto, cuántas unidades y a qué precio.</summary>
public class PedidoItem
{
    public int Id { get; set; }

    public int PedidoId { get; set; }

    public Pedido? Pedido { get; set; }

    public int ProductoId { get; set; }

    public Producto? Producto { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
    public int Cantidad { get; set; }

    /// <summary>
    /// Copia del precio del producto al momento de la compra: si el precio cambia
    /// después, los pedidos viejos no se alteran.
    /// </summary>
    public decimal PrecioUnitario { get; set; }

    /// <summary>Se calcula, no se guarda.</summary>
    [NotMapped]
    public decimal Subtotal => PrecioUnitario * Cantidad;
}