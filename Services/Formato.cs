using System.Globalization;
using Blazor_Ecommerce.Models;

namespace Blazor_Ecommerce.Services;

/// <summary>Funciones de formato compartidas por las pantallas de la tienda.</summary>
public static class Formato
{
    private static readonly CultureInfo Cultura = new("es-AR");
    private static readonly TimeZoneInfo ZonaArgentina = BuscarZonaArgentina();

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

    /// <summary>Convierte una fecha guardada en UTC a hora de Argentina, por ejemplo "02/11/2026 14:35".</summary>
    /// <param name="fechaUtc">Fecha y hora en UTC.</param>
    /// <returns>La fecha y hora local formateadas.</returns>
    public static string FechaHora(DateTime fechaUtc)
    {
        var utc = DateTime.SpecifyKind(fechaUtc, DateTimeKind.Utc);

        return TimeZoneInfo.ConvertTimeFromUtc(utc, ZonaArgentina).ToString("dd/MM/yyyy HH:mm", Cultura);
    }

    /// <summary>Devuelve el nombre legible de un motivo de movimiento (con tilde donde corresponde).</summary>
    /// <param name="motivo">Motivo del movimiento.</param>
    /// <returns>El texto para mostrar en pantalla.</returns>
    public static string Motivo(MotivoMovimiento motivo) => motivo switch
    {
        MotivoMovimiento.Compra => "Compra",
        MotivoMovimiento.Venta => "Venta",
        MotivoMovimiento.Cancelacion => "Cancelación",
        MotivoMovimiento.Merma => "Merma",
        MotivoMovimiento.Ajuste => "Ajuste",
        _ => motivo.ToString()
    };

    /// <summary>Muestra una variación de stock con su signo, por ejemplo "+10" o "-3".</summary>
    /// <param name="variacion">Cambio neto en unidades.</param>
    /// <returns>El número con signo explícito cuando es positivo.</returns>
    public static string Variacion(int variacion) =>
        variacion > 0 ? $"+{variacion}" : variacion.ToString(CultureInfo.InvariantCulture);

    // Windows e IANA nombran distinto a la misma zona; se prueban ambos y, si ninguno existe, se usa UTC-3 fijo.
    private static TimeZoneInfo BuscarZonaArgentina()
    {
        foreach (var id in new[] { "America/Argentina/Buenos_Aires", "Argentina Standard Time" })
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(id);
            }
            catch (TimeZoneNotFoundException)
            {
            }
            catch (InvalidTimeZoneException)
            {
            }
        }

        return TimeZoneInfo.CreateCustomTimeZone("ART", TimeSpan.FromHours(-3), "Argentina", "Argentina");
    }
}