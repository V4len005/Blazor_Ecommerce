using Blazor_Ecommerce.Models;

namespace Blazor_Ecommerce.Services;

/// <summary>Producto dentro del carrito, con los datos necesarios para mostrarlo.</summary>
public class ItemCarrito
{
    public int ProductoId { get; init; }

    public string Nombre { get; set; } = string.Empty;

    public string Marca { get; set; } = string.Empty;

    /// <summary>Precio visto al agregar el producto. Es informativo: el servidor recalcula al confirmar.</summary>
    public decimal PrecioUnitario { get; set; }

    public int Cantidad { get; set; }

    /// <summary>Stock que había cuando se agregó o actualizó el producto.</summary>
    public int StockDisponible { get; set; }

    public decimal Subtotal => PrecioUnitario * Cantidad;
}

/// <summary>Resultado de una operación sobre el carrito: si salió bien y, si no, el motivo.</summary>
/// <param name="Exito">True si la operación se realizó.</param>
/// <param name="Mensaje">Motivo del rechazo (nulo cuando salió bien).</param>
public record ResultadoCarrito(bool Exito, string? Mensaje = null);

/// <summary>
/// Estado compartido del carrito de compras. Se registra como Scoped: hay una instancia por
/// conexión (circuito) de cada visitante. Avisa a los componentes suscriptos cuando cambia.
/// </summary>
public class CarritoState
{
    private readonly List<ItemCarrito> _items = new();

    /// <summary>Productos que hay en el carrito (solo lectura).</summary>
    public IReadOnlyList<ItemCarrito> Items => _items;

    /// <summary>Cantidad total de unidades (suma de todas las cantidades).</summary>
    public int CantidadTotal => _items.Sum(i => i.Cantidad);

    /// <summary>Total informativo del carrito. El total real lo calcula el servidor al confirmar.</summary>
    public decimal Total => _items.Sum(i => i.Subtotal);

    public bool EstaVacio => _items.Count == 0;

    /// <summary>Se dispara cada vez que el contenido del carrito cambia.</summary>
    public event Action? OnChange;

    /// <summary>Agrega unidades de un producto, sin superar el stock que se conoce.</summary>
    /// <param name="producto">Producto a agregar.</param>
    /// <param name="cantidad">Unidades a agregar (mayor a 0).</param>
    /// <returns>El resultado; si se rechaza, trae el motivo para mostrar.</returns>
    public ResultadoCarrito Agregar(Producto producto, int cantidad = 1)
    {
        if (cantidad <= 0)
        {
            return new(false, "La cantidad debe ser mayor a 0.");
        }

        if (!producto.Activo)
        {
            return new(false, "El producto no está disponible.");
        }

        if (producto.StockActual <= 0)
        {
            return new(false, "El producto no tiene stock.");
        }

        var item = _items.FirstOrDefault(i => i.ProductoId == producto.Id);
        var enCarrito = item?.Cantidad ?? 0;

        if (enCarrito + cantidad > producto.StockActual)
        {
            var restantes = producto.StockActual - enCarrito;

            return new(false, restantes <= 0
                ? $"Ya tenés en el carrito todo el stock disponible ({producto.StockActual})."
                : $"Solo podés agregar {restantes} unidad(es) más: hay {producto.StockActual} en stock.");
        }

        if (item is null)
        {
            _items.Add(new ItemCarrito
            {
                ProductoId = producto.Id,
                Nombre = producto.Nombre,
                Marca = producto.Marca,
                PrecioUnitario = producto.Precio,
                Cantidad = cantidad,
                StockDisponible = producto.StockActual
            });
        }
        else
        {
            item.Cantidad += cantidad;
            item.PrecioUnitario = producto.Precio;
            item.StockDisponible = producto.StockActual;
        }

        NotificarCambio();

        return new(true);
    }

    /// <summary>Fija la cantidad de un producto del carrito. Con 0 o menos, lo quita.</summary>
    /// <param name="productoId">Producto a modificar.</param>
    /// <param name="cantidad">Nueva cantidad total.</param>
    /// <returns>El resultado; si se rechaza, trae el motivo para mostrar.</returns>
    public ResultadoCarrito CambiarCantidad(int productoId, int cantidad)
    {
        var item = _items.FirstOrDefault(i => i.ProductoId == productoId);

        if (item is null)
        {
            return new(false, "El producto ya no está en el carrito.");
        }

        if (cantidad <= 0)
        {
            Quitar(productoId);
            return new(true);
        }

        if (cantidad > item.StockDisponible)
        {
            return new(false, $"Solo hay {item.StockDisponible} unidad(es) disponibles de \"{item.Nombre}\".");
        }

        if (cantidad != item.Cantidad)
        {
            item.Cantidad = cantidad;
            NotificarCambio();
        }

        return new(true);
    }

    /// <summary>Quita un producto del carrito.</summary>
    /// <param name="productoId">Producto a quitar.</param>
    public void Quitar(int productoId)
    {
        if (_items.RemoveAll(i => i.ProductoId == productoId) > 0)
        {
            NotificarCambio();
        }
    }

    /// <summary>Vacía el carrito.</summary>
    public void Vaciar()
    {
        if (_items.Count > 0)
        {
            _items.Clear();
            NotificarCambio();
        }
    }

    private void NotificarCambio() => OnChange?.Invoke();
}