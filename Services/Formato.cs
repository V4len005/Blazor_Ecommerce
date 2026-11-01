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

    /// <summary>Devuelve la clase de Bootstrap Icons que representa a una categoría.</summary>
    /// <param name="categoria">Nombre de la categoría (puede ser nulo).</param>
    /// <returns>Una clase como "bi-laptop"; si no se reconoce la categoría, un ícono genérico.</returns>
    public static string IconoCategoria(string? categoria) => categoria switch
    {
        "Consolas" => "bi-controller",
        "Notebooks" => "bi-laptop",
        "Teclados y Mouse" => "bi-keyboard",
        "Auriculares" => "bi-headphones",
        "Monitores" => "bi-display",
        _ => "bi-box-seam"
    };
}