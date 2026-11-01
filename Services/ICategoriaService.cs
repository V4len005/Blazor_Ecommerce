using Blazor_Ecommerce.Models;

namespace Blazor_Ecommerce.Services;

/// <summary>Operaciones de consulta sobre las categorías de la tienda.</summary>
public interface ICategoriaService
{
    /// <summary>Obtiene todas las categorías ordenadas por nombre.</summary>
    /// <returns>Lista de categorías.</returns>
    Task<List<Categoria>> ObtenerTodas();
}
