using Blazor_Ecommerce.Data;
using Blazor_Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Blazor_Ecommerce.Services;

/// <summary>Implementación de <see cref="ICategoriaService"/> con EF Core.</summary>
public class CategoriaService : ICategoriaService
{
    private readonly IDbContextFactory<TiendaDbContext> _fabrica;

    public CategoriaService(IDbContextFactory<TiendaDbContext> fabrica)
    {
        _fabrica = fabrica;
    }

    public async Task<List<Categoria>> ObtenerTodas()
    {
        await using var contexto = await _fabrica.CreateDbContextAsync();

        return await contexto.Categorias
            .AsNoTracking()
            .OrderBy(c => c.Nombre)
            .ToListAsync();
    }
}
