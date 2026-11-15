using System.Text;
using Blazor_Ecommerce.Models;

namespace Blazor_Ecommerce.Services;

/// <summary>Arma textos de un pedido listos para copiar y pegar.</summary>
public static class TextoPedido
{
    /// <summary>Arma un resumen en texto plano de un pedido.</summary>
    /// <param name="pedido">Pedido con sus ítems y productos cargados.</param>
    /// <param name="incluirCliente">Si es true, agrega los datos del cliente (solo para administración).</param>
    /// <returns>El resumen, una línea por dato.</returns>
    public static string Resumen(Pedido pedido, bool incluirCliente)
    {
        var texto = new StringBuilder();

        texto.AppendLine($"Pedido #{pedido.Id} - {Tienda.Nombre}");
        texto.AppendLine($"Fecha: {Formato.FechaHora(pedido.Fecha)}");
        texto.AppendLine($"Estado: {pedido.Estado}");

        if (incluirCliente)
        {
            var entrega = string.IsNullOrWhiteSpace(pedido.DireccionEnvio)
                ? "Retiro en el local"
                : pedido.DireccionEnvio;

            texto.AppendLine($"Cliente: {pedido.NombreCliente} ({pedido.Email})");
            texto.AppendLine($"Entrega: {entrega}");
        }

        texto.AppendLine();

        foreach (var item in pedido.Items)
        {
            var nombre = item.Producto?.Nombre ?? "Producto";
            texto.AppendLine($"{item.Cantidad} x {nombre} - {Formato.Precio(item.Subtotal)}");
        }

        texto.AppendLine();
        texto.Append($"Total: {Formato.Precio(pedido.Total)}");

        return texto.ToString();
    }
}