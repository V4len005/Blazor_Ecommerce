using System.ComponentModel.DataAnnotations;

namespace Blazor_Ecommerce.Models;

/// <summary>Compra confirmada por un cliente. No hay usuarios: sus datos se guardan en el propio pedido.</summary>
public class Pedido
{
    public int Id { get; set; }

    /// <summary>Momento de la confirmación, en hora UTC.</summary>
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    public EstadoPedido Estado { get; set; } = EstadoPedido.Confirmado;

    /// <summary>Suma de los subtotales, calculada por el servidor con los precios de la base.</summary>
    public decimal Total { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
    public string NombreCliente { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
    [StringLength(150, ErrorMessage = "El email no puede superar los 150 caracteres")]
    public string Email { get; set; } = string.Empty;

    [StringLength(30, ErrorMessage = "El teléfono no puede superar los 30 caracteres")]
    public string? Telefono { get; set; }

    /// <summary>Dirección de entrega. Si es nula, el cliente retira el pedido en el local.</summary>
    [StringLength(200, ErrorMessage = "La dirección no puede superar los 200 caracteres")]
    public string? DireccionEnvio { get; set; }

    [StringLength(500, ErrorMessage = "Las observaciones no pueden superar los 500 caracteres")]
    public string? Observaciones { get; set; }

    /// <summary>Productos comprados (lado "muchos" de la relación).</summary>
    public ICollection<PedidoItem> Items { get; set; } = new List<PedidoItem>();
}