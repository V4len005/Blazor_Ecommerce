using System.ComponentModel.DataAnnotations;

namespace Blazor_Ecommerce.Models;

/// <summary>Agrupa productos de la tienda (por ejemplo: Consolas, Notebooks).</summary>
public class Categoria
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre de la categoría es obligatorio")]
    [StringLength(80, ErrorMessage = "El nombre no puede superar los 80 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(300, ErrorMessage = "La descripción no puede superar los 300 caracteres")]
    public string? Descripcion { get; set; }

    /// <summary>Productos que pertenecen a esta categoría (lado "muchos" de la relación).</summary>
    public ICollection<Producto> Productos { get; set; } = new List<Producto>();
}