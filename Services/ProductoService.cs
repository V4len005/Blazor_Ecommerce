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
        await using var contexto = await _fabrica.CreateDbContextAsync();

        // Solo se guarda la clave foránea; la categoría ya existe en la base.
        producto.Categoria = null;

        contexto.Productos.Add(producto);
        await contexto.SaveChangesAsync();

        return producto;
    }

    public async Task<Producto> Actualizar(Producto producto)
    {
        await using var contexto = await _fabrica.CreateDbContextAsync();

        var existente = await contexto.Productos.FindAsync(producto.Id);

        if (existente is null)
        {
            throw new InvalidOperationException("El producto ya no existe.");
        }

        // Copia solo las propiedades simples; EF detecta cuáles cambiaron.
        contexto.Entry(existente).CurrentValues.SetValues(producto);
        await contexto.SaveChangesAsync();

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

    // En LIKE, los caracteres [ % _ tienen significado especial: se escapan entre corchetes.
    private static string EscaparLike(string texto) =>
        texto.Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");
}
