using Blazor_Ecommerce.Data;
using Blazor_Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Blazor_Ecommerce.Services;

/// <summary>Implementación de <see cref="IProductoService"/> con EF Core.</summary>
public class ProductoService : IProductoService
{
    private readonly IDbContextFactory<TiendaDbContext> _fabrica;

    public ProductoService(IDbContextFactory<TiendaDbContext> fabrica)
    {
        _fabrica = fabrica;
    }

    public async Task<ResultadoPaginado<Producto>> ObtenerPaginado(
        string? busqueda, int? categoriaId, int pagina, int tamanioPagina, bool soloActivos = false)
    {
        await using var contexto = await _fabrica.CreateDbContextAsync();

        IQueryable<Producto> consulta = contexto.Productos.AsNoTracking();

        if (soloActivos)
        {
            consulta = consulta.Where(p => p.Activo);
        }

        if (categoriaId.HasValue)
        {
            consulta = consulta.Where(p => p.CategoriaId == categoriaId.Value);
        }

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            var patron = $"%{EscaparLike(busqueda.Trim())}%";

            consulta = consulta.Where(p =>
                EF.Functions.Like(p.Nombre, patron) ||
                EF.Functions.Like(p.Marca, patron) ||
                (p.Sku != null && EF.Functions.Like(p.Sku, patron)));
        }

        var total = await consulta.CountAsync();

        var tamanio = Math.Max(1, tamanioPagina);
        var totalPaginas = Math.Max(1, (int)Math.Ceiling(total / (double)tamanio));
        var paginaValida = Math.Clamp(pagina, 1, totalPaginas);

        var items = await consulta
            .Include(p => p.Categoria)
            .OrderBy(p => p.Nombre)
            .ThenBy(p => p.Id)
            .Skip((paginaValida - 1) * tamanio)
            .Take(tamanio)
            .ToListAsync();

        return new ResultadoPaginado<Producto>
        {
            Items = items,
            TotalRegistros = total,
            Pagina = paginaValida,
            TamanioPagina = tamanio
        };
    }

    public async Task<List<Producto>> ObtenerRecientes(int cantidad)
    {
        await using var contexto = await _fabrica.CreateDbContextAsync();

        return await contexto.Productos
            .AsNoTracking()
            .Where(p => p.Activo)
            .Include(p => p.Categoria)
            .OrderByDescending(p => p.Id)
            .Take(Math.Max(1, cantidad))
            .ToListAsync();
    }

    public async Task<Producto?> ObtenerPorId(int id)
    {
        await using var contexto = await _fabrica.CreateDbContextAsync();

        return await contexto.Productos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Producto> Crear(Producto producto)
    {
        Normalizar(producto);
        ValidarReglasDeNegocio(producto);

        await using var contexto = await _fabrica.CreateDbContextAsync();

        await ValidarSkuUnico(contexto, producto);

        // Solo se guarda la clave foránea; la categoría ya existe en la base.
        producto.Categoria = null;

        contexto.Productos.Add(producto);
        await GuardarCambios(contexto);

        return producto;
    }

    public async Task<Producto> Actualizar(Producto producto)
    {
        Normalizar(producto);
        ValidarReglasDeNegocio(producto);

        await using var contexto = await _fabrica.CreateDbContextAsync();

        var existente = await contexto.Productos.FindAsync(producto.Id);

        if (existente is null)
        {
            throw new InvalidOperationException("El producto ya no existe.");
        }

        await ValidarSkuUnico(contexto, producto);

        // El stock no se edita desde acá: se conserva el valor que hay en la base.
        var stockActual = existente.StockActual;

        // Copia solo las propiedades simples; EF detecta cuáles cambiaron.
        contexto.Entry(existente).CurrentValues.SetValues(producto);
        existente.StockActual = stockActual;

        await GuardarCambios(contexto);

        return existente;
    }

    public async Task CambiarActivo(int id)
    {
        await using var contexto = await _fabrica.CreateDbContextAsync();

        var producto = await contexto.Productos.FindAsync(id);

        if (producto is null)
        {
            throw new InvalidOperationException("El producto ya no existe.");
        }

        producto.Activo = !producto.Activo;
        await contexto.SaveChangesAsync();
    }

    public async Task Eliminar(int id)
    {
        await using var contexto = await _fabrica.CreateDbContextAsync();

        var producto = await contexto.Productos.FindAsync(id);

        if (producto is null)
        {
            return;
        }

        contexto.Productos.Remove(producto);

        try
        {
            await contexto.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException(
                "No se puede eliminar el producto porque tiene registros asociados (por ejemplo, pedidos). Desactivalo en su lugar.",
                ex);
        }
    }

    // Limpia espacios y convierte los textos opcionales vacíos en null.
    private static void Normalizar(Producto producto)
    {
        producto.Nombre = producto.Nombre.Trim();
        producto.Marca = producto.Marca.Trim();
        producto.Sku = string.IsNullOrWhiteSpace(producto.Sku) ? null : producto.Sku.Trim();
        producto.ImagenUrl = string.IsNullOrWhiteSpace(producto.ImagenUrl) ? null : producto.ImagenUrl.Trim();
    }

    // El servidor no confía en el formulario: vuelve a comprobar las reglas de negocio.
    private static void ValidarReglasDeNegocio(Producto producto)
    {
        if (producto.Precio <= 0)
        {
            throw new InvalidOperationException("El precio debe ser mayor a 0.");
        }

        if (producto.StockActual < 0 || producto.StockMinimo < 0)
        {
            throw new InvalidOperationException("El stock no puede ser negativo.");
        }

        if (producto.CategoriaId <= 0)
        {
            throw new InvalidOperationException("Seleccioná una categoría.");
        }
    }

    private static async Task ValidarSkuUnico(TiendaDbContext contexto, Producto producto)
    {
        if (producto.Sku is null)
        {
            return;
        }

        var repetido = await contexto.Productos
            .AnyAsync(p => p.Sku == producto.Sku && p.Id != producto.Id);

        if (repetido)
        {
            throw new InvalidOperationException($"Ya existe otro producto con el SKU {producto.Sku}.");
        }
    }

    private static async Task GuardarCambios(TiendaDbContext contexto)
    {
        try
        {
            await contexto.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new InvalidOperationException(
                "No se pudo guardar el producto. Verificá que la categoría exista y que los datos sean válidos.",
                ex);
        }
    }

    // En LIKE, los caracteres [ % _ tienen significado especial: se escapan entre corchetes.
    private static string EscaparLike(string texto) =>
        texto.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");
}