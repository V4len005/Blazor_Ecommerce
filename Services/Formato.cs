using System.Globalization;

namespace Blazor_Ecommerce.Services;

/// <summary>Funciones de formato compartidas por las pantallas de la tienda.</summary>
public static class Formato
{
    private static readonly CultureInfo Cultura = new("es-AR");

    /// <summary>Convierte un importe a texto en pesos argentinos, por ejemplo "$ 1.100.000,00".</summary>
    /// <param name="valor">Importe a mostrar.</param>
    /// <returns>El importe formateado como moneda.</returns>
    public static string Precio(decimal valor) => valor.ToString("C", Cultura);
}