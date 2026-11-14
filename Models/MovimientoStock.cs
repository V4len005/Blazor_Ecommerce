using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blazor_Ecommerce.Models;

/// <summary>Registro de un cambio en el stock de un producto.</summary>
public class MovimientoStock
{
    public int Id { get; set; }

    public int ProductoId { get; set; }

    /// <summary>Producto afectado (propiedad de navegación).</summary>
    public Producto? Producto { get; set; }

    public TipoMovimiento Tipo { get; set; }

    public MotivoMovimiento Motivo { get; set; }

    /// <summary>Unidades movidas. En Entrada y Salida es positiva; en Ajuste puede ser negativa.</summary>
    public int Cantidad { get; set; }

    /// <summary>Momento del movimiento, en hora UTC.</summary>
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    /// <summary>Detalle libre, por ejemplo "Venta online" o "Stock inicial".</summary>
    [StringLength(200, ErrorMessage = "La observación no puede superar los 200 caracteres")]
    public string? Observacion { get; set; }

    /// <summary>Pedido que originó el movimiento (ventas y cancelaciones). Nulo en los movimientos manuales.</summary>
    public int? PedidoId { get; set; }

    public Pedido? Pedido { get; set; }

    /// <summary>Cambio neto en el stock: negativo en las Salidas, positivo (o con su signo) en el resto.</summary>
    [NotMapped]
    public int Variacion => Tipo == TipoMovimiento.Salida ? -Cantidad : Cantidad;
}