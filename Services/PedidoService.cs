using System.ComponentModel.DataAnnotations;
using Blazor_Ecommerce.Data;
using Blazor_Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Blazor_Ecommerce.Services;

/// <summary>Implementación de <see cref="IPedidoService"/> con EF Core.</summary>
public class PedidoService : IPedidoService
{
    private readonly IDbContextFactory<TiendaDbContext> _fabrica;

    public PedidoService(IDbContextFactory<TiendaDbContext> fabrica)
    {
        _fabrica = fabrica;
    }

    public async Task<Pedido> ConfirmarPedido(SolicitudPedido solicitud)
    {
        var lineas = Consolidar(solicitud.Lineas);

        var pedido = new Pedido
        {
            Fecha = DateTime.UtcNow,
            Estado = EstadoPedido.Confirmado,
            NombreCliente = (solicitud.NombreCliente ?? string.Empty).Trim(),
            Email = (solicitud.Email ?? string.Empty).Trim(),
            Telefono = LimpiarOpcional(solicitud.Telefono),
            DireccionEnvio = LimpiarOpcional(solicitud.DireccionEnvio),
            Observaciones = LimpiarOpcional(solicitud.Observaciones)
        };

        ValidarDatos(pedido);

        await using var contexto = await _fabrica.CreateDbContextAsync();

        // Productos con seguimiento activo: su stock va a cambiar en esta transacción.
        var ids = lineas.Select(l => l.ProductoId).ToList();
        var productos = await contexto.Productos
            .Where(p => ids.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id);

        foreach (var linea in lineas)
        {
            if (!productos.TryGetValue(linea.ProductoId, out var producto) || !producto.Activo)
            {
                throw new InvalidOperationException(
                    "Uno de los productos de tu carrito ya no está disponible. Revisá tu carrito.");
            }

            // El precio sale de la base, no del carrito.
            pedido.Items.Add(new PedidoItem
            {
                Producto = producto,
                Cantidad = linea.Cantidad,
                PrecioUnitario = producto.Precio
            });

            // Descuenta el stock (y falla si no alcanza) y agrega el movimiento, sin guardar todavía.
            AplicadorStock.Aplicar(
                contexto, producto, TipoMovimiento.Salida, MotivoMovimiento.Venta,
                linea.Cantidad, "Venta online", pedido);
        }

        pedido.Total = pedido.Items.Sum(i => i.Subtotal);
        contexto.Pedidos.Add(pedido);

        try
        {
            // Un único SaveChanges: pedido, ítems, stock y movimientos se guardan juntos o no se guarda nada.
            await contexto.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new InvalidOperationException(
                "El stock de algún producto cambió mientras confirmabas. Revisá tu carrito e intentá de nuevo.", ex);
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException(
                "No se pudo confirmar el pedido. Intentá de nuevo en unos minutos.", ex);
        }

        return pedido;
    }

    public async Task<Pedido?> ObtenerPorId(int id)
    {
        await using var contexto = await _fabrica.CreateDbContextAsync();

        return await contexto.Pedidos
            .AsNoTracking()
            .Include(p => p.Items)
                .ThenInclude(i => i.Producto)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    // Une las líneas repetidas del mismo producto y descarta cantidades inválidas.
    private static List<LineaPedido> Consolidar(IReadOnlyList<LineaPedido>? lineas)
    {
        if (lineas is null || lineas.Count == 0)
        {
            throw new InvalidOperationException("El carrito está vacío.");
        }

        if (lineas.Any(l => l.Cantidad <= 0))
        {
            throw new InvalidOperationException("Las cantidades del pedido deben ser mayores a 0.");
        }

        return lineas
            .GroupBy(l => l.ProductoId)
            .Select(g => new LineaPedido(g.Key, g.Sum(l => l.Cantidad)))
            .ToList();
    }

    // Vuelve a aplicar las anotaciones del modelo: el servidor no confía en el formulario.
    private static void ValidarDatos(Pedido pedido)
    {
        var errores = new List<ValidationResult>();

        if (!Validator.TryValidateObject(pedido, new ValidationContext(pedido), errores, validateAllProperties: true))
        {
            throw new InvalidOperationException(
                errores[0].ErrorMessage ?? "Los datos del pedido no son válidos.");
        }
    }

    private static string? LimpiarOpcional(string? texto) =>
        string.IsNullOrWhiteSpace(texto) ? null : texto.Trim();
}