using System.ComponentModel.DataAnnotations;

namespace Blazor_Ecommerce.Models;

/// <summary>Artículo a la venta en la tienda.</summary>
public class Producto : IValidatableObject
{
    /// <summary>Precio desde el cual el SKU pasa a ser obligatorio (producto de alto valor).</summary>
    private const decimal UmbralAltoValor = 1_000_000m;

    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(120, ErrorMessage = "El nombre no puede superar los 120 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La marca es obligatoria")]
    [StringLength(60, ErrorMessage = "La marca no puede superar los 60 caracteres")]
    public string Marca { get; set; } = string.Empty;

    /// <summary>Código interno del producto (opcional). Formato AAA-0000.</summary>
    [StringLength(30, ErrorMessage = "El SKU no puede superar los 30 caracteres")]
    [CustomValidation(typeof(ValidacionesProducto), nameof(ValidacionesProducto.ValidarFormatoSku))]
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
    [Url(ErrorMessage = "La imagen debe ser una URL válida, por ejemplo https://sitio.com/foto.jpg")]
    public string? ImagenUrl { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Seleccioná una categoría")]
    public int CategoriaId { get; set; }

    /// <summary>Categoría a la que pertenece (propiedad de navegación).</summary>
    public Categoria? Categoria { get; set; }

    /// <summary>
    /// Reglas que dependen de más de un campo. Se ejecuta solo cuando
    /// todos los atributos de cada propiedad ya pasaron la validación.
    /// </summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Regla 1: un producto de alto valor necesita SKU para su trazabilidad.
        if (Precio >= UmbralAltoValor && string.IsNullOrWhiteSpace(Sku))
        {
            yield return new ValidationResult(
                "Los productos de $ 1.000.000 o más requieren un SKU para su trazabilidad.",
                new[] { nameof(Sku) });
        }

        // Regla 2: el nombre no puede ser solamente la marca.
        if (!string.IsNullOrWhiteSpace(Nombre) &&
            string.Equals(Nombre.Trim(), Marca?.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            yield return new ValidationResult(
                "El nombre no puede ser igual a la marca. Agregá el modelo o la característica principal.",
                new[] { nameof(Nombre) });
        }
    }
}