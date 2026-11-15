using Blazor_Ecommerce.Data;
using Blazor_Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Blazor_Ecommerce.Services;

/// <summary>Implementación de <see cref="ICategoriaService"/> con EF Core.</summary>
public class CategoriaService : ICategoriaService
{
    private readonly IDbContextFactory<TiendaDbContext> _fabrica;
    private readonly ILogger<CategoriaService> _logger;

    public CategoriaService(IDbContextFactory<TiendaDbContext> fabrica, ILogger<CategoriaService> logger)
    {
        _fabrica = fabrica;
        _logger = logger;
    }

    public async Task<List<Categoria>> ObtenerTodas()
    {
        await using var contexto = await _fabrica.CreateDbContextAsync();

        var categorias = await contexto.Categorias
            .AsNoTracking()
            .OrderBy(c => c.Nombre)
            .ToListAsync();

        _logger.LogDebug("Se consultaron {Cantidad} categorías.", categorias.Count);

        return categorias;
    }
}