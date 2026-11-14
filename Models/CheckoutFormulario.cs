using System.ComponentModel.DataAnnotations;

namespace Blazor_Ecommerce.Models;

/// <summary>Datos que el cliente completa en el checkout para confirmar su pedido.</summary>
public class CheckoutFormulario : IValidatableObject
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
    public string NombreCliente { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "El email no tiene un formato válido")]
    [StringLength(150, ErrorMessage = "El email no puede superar los 150 caracteres")]
    public string Email { get; set; } = string.Empty;

    [StringLength(30, ErrorMessage = "El teléfono no puede superar los 30 caracteres")]
    public string? Telefono { get; set; }

    public TipoEntrega Entrega { get; set; } = TipoEntrega.Retiro;

    [StringLength(200, ErrorMessage = "La dirección no puede superar los 200 caracteres")]
    public string? DireccionEnvio { get; set; }

    [StringLength(500, ErrorMessage = "Las observaciones no pueden superar los 500 caracteres")]
    public string? Observaciones { get; set; }

    /// <summary>
    /// Reglas que dependen de más de un campo. Se ejecuta solo cuando
    /// todos los atributos de cada propiedad ya pasaron la validación.
    /// </summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Regla 1: si la entrega es a domicilio, la dirección es obligatoria.
        if (Entrega == TipoEntrega.Domicilio && (DireccionEnvio ?? string.Empty).Trim().Length < 5)
        {
            yield return new ValidationResult(
                "Si la entrega es a domicilio, ingresá la dirección (calle, número y localidad).",
                new[] { nameof(DireccionEnvio) });
        }

        // Regla 2: si se informa un teléfono, debe tener entre 8 y 15 dígitos (se ignoran espacios y guiones).
        if (!string.IsNullOrWhiteSpace(Telefono))
        {
            var digitos = Telefono.Count(char.IsDigit);

            if (digitos is < 8 or > 15)
            {
                yield return new ValidationResult(
                    "El teléfono debe tener entre 8 y 15 dígitos.",
                    new[] { nameof(Telefono) });
            }
        }
    }
}