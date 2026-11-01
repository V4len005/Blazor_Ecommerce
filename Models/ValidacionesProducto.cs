using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Blazor_Ecommerce.Models;

/// <summary>Reglas de validación reutilizables para los datos de un producto.</summary>
public static class ValidacionesProducto
{
    // Tres letras mayúsculas, un guion y cuatro números. Ejemplo: NOT-0001.
    private static readonly Regex PatronSku = new(@"^[A-Z]{3}-\d{4}$", RegexOptions.Compiled);

    /// <summary>Valida el formato del SKU. Un SKU vacío es válido porque el campo es opcional.</summary>
    /// <param name="sku">Valor ingresado.</param>
    /// <param name="contexto">Contexto con el nombre del campo validado.</param>
    /// <returns>El resultado de la validación.</returns>
    public static ValidationResult? ValidarFormatoSku(string? sku, ValidationContext contexto)
    {
        if (string.IsNullOrEmpty(sku) || PatronSku.IsMatch(sku))
        {
            return ValidationResult.Success;
        }

        var miembros = contexto.MemberName is null ? null : new[] { contexto.MemberName };

        return new ValidationResult(
            "El SKU debe tener el formato AAA-0000 (tres letras mayúsculas, un guion y cuatro números). Ejemplo: NOT-0001.",
            miembros);
    }
}