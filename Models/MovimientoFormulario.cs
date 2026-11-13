using System.ComponentModel.DataAnnotations;

namespace Blazor_Ecommerce.Models;

/// <summary>Datos que se cargan en el formulario para registrar un movimiento de stock.</summary>
public class MovimientoFormulario : IValidatableObject
{
    public OperacionStock Operacion { get; set; } = OperacionStock.Entrada;

    /// <summary>Unidades que entran o salen (para Entrada y Merma).</summary>
    public int? Cantidad { get; set; }

    /// <summary>Cantidad real contada en el depósito (para Ajuste).</summary>
    public int? StockReal { get; set; }

    [StringLength(200, ErrorMessage = "La observación no puede superar los 200 caracteres")]
    public string? Observacion { get; set; }

    /// <summary>Stock actual del producto. Lo completa la pantalla; no es un campo del formulario.</summary>
    public int StockActual { get; set; }

    /// <summary>Reglas que dependen de la operación elegida y del stock actual.</summary>
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Operacion == OperacionStock.Ajuste)
        {
            if (StockReal is null or < 0)
            {
                yield return new ValidationResult(
                    "Ingresá el stock real contado (0 o más).",
                    new[] { nameof(StockReal) });
            }
            else if (StockReal == StockActual)
            {
                yield return new ValidationResult(
                    "El stock real coincide con el actual: no hay nada que ajustar.",
                    new[] { nameof(StockReal) });
            }

            yield break;
        }

        if (Cantidad is null or <= 0)
        {
            yield return new ValidationResult(
                "La cantidad debe ser mayor a 0.",
                new[] { nameof(Cantidad) });
        }
        else if (Operacion == OperacionStock.Merma && Cantidad > StockActual)
        {
            yield return new ValidationResult(
                $"No podés descartar más de {StockActual} unidad(es), que es el stock actual.",
                new[] { nameof(Cantidad) });
        }
    }
}