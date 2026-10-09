using System.ComponentModel.DataAnnotations;

namespace Blazor_Ecommerce.Models;

/// <summary>Artículo a la venta en la tienda.</summary>
public class Producto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(120, ErrorMessage = "El nombre no puede superar los 120 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La marca es obligatoria")]
    [StringLength(60, ErrorMessage = "La marca no puede superar los 60 caracteres")]
    public string Marca { get; set; } = string.Empty;

    /// <summary>Código interno del producto (opcional). Su formato se valida en el Módulo 3.</summary>
    [StringLength(30, ErrorMessage = "El SKU no puede superar los 30 caracteres")]
    public string? Sku { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria")]
    [StringLength(1000, ErrorMessage = "La descripción no puede superar los 1000 caracteres")]
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>Texto libre con las características técnicas, por ejemplo "16 GB RAM / 512 GB SSD".</summary>
    [Required(ErrorMessage = "Las especificaciones son obligatorias")]
    [StringLength(500, ErrorMessage = "Las especificaciones no pueden superar los 500 caracteres")]
    public string Especificaciones { get; set; } = string.Empty;

    [Range(0.01, 99999999.99, ErrorMessage = "El precio debe ser mayor a 0")]
    public decimal Precio { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
    public int StockActual { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "El stock mínimo no puede ser negativo")]
    public int StockMinimo { get; set; }

    /// <summary>Si es false, el producto no aparece en el catálogo público.</summary>
    public bool Activo { get; set; } = true;

    /// <summary>URL de una imagen (solo texto). Si está vacía se muestra un ícono.</summary>
    [StringLength(500, ErrorMessage = "La URL no puede superar los 500 caracteres")]
    public string? ImagenUrl { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Seleccioná una categoría")]
    public int CategoriaId { get; set; }

    /// <summary>Categoría a la que pertenece (propiedad de navegación).</summary>
    public Categoria? Categoria { get; set; }
}