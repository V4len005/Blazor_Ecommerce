using Blazor_Ecommerce.Models;

namespace Blazor_Ecommerce.Services;

/// <summary>Operaciones de consulta y mantenimiento sobre los productos de la tienda.</summary>
public interface IProductoService
{
    /// <summary>Obtiene una página de productos, con búsqueda y filtro opcionales.</summary>
    /// <param name="busqueda">Texto a buscar en nombre, marca o SKU (puede ser nulo o vacío).</param>
    /// <param name="categoriaId">Si tiene valor, solo devuelve productos de esa categoría.</param>
    /// <param name="pagina">Número de página pedido (empieza en 1; se ajusta si queda fuera de rango).</param>
    /// <param name="tamanioPagina">Cantidad de productos por página.</param>
    /// <param name="soloActivos">Si es true, excluye los productos desactivados.</param>
    /// <returns>La página de productos con sus totales.</returns>
    Task<ResultadoPaginado<Producto>> ObtenerPaginado(
        string? busqueda, int? categoriaId, int pagina, int tamanioPagina, bool soloActivos = false);

    /// <summary>Obtiene los productos activos agregados más recientemente.</summary>
    /// <param name="cantidad">Cantidad máxima de productos a devolver (mínimo 1).</param>
    /// <returns>Lista de productos con su categoría, del más nuevo al más antiguo.</returns>
    Task<List<Producto>> ObtenerRecientes(int cantidad);

    /// <summary>Busca un producto por su identificador.</summary>
    /// <param name="id">Identificador del producto.</param>
    /// <returns>El producto, o null si no existe.</returns>
    Task<Producto?> ObtenerPorId(int id);

    /// <summary>Crea un producto nuevo.</summary>
    /// <param name="producto">Datos del producto.</param>
    /// <returns>El producto creado, con su Id asignado.</returns>
    /// <exception cref="InvalidOperationException">
    /// Si incumple una regla de negocio (precio, stock, categoría), si el SKU ya existe
    /// o si falla el guardado.
    /// </exception>
    Task<Producto> Crear(Producto producto);

    /// <summary>
    /// Actualiza los datos de un producto existente. No modifica el stock actual:
    /// el stock cambia solo mediante movimientos de stock y pedidos.
    /// </summary>
    /// <param name="producto">Producto con los valores nuevos (identificado por su Id).</param>
    /// <returns>El producto actualizado.</returns>
    /// <exception cref="InvalidOperationException">
    /// Si el producto ya no existe, si incumple una regla de negocio, si el SKU
    /// pertenece a otro producto o si falla el guardado.
    /// </exception>
    Task<Producto> Actualizar(Producto producto);

    /// <summary>Activa un producto desactivado o desactiva uno activo.</summary>
    /// <param name="id">Identificador del producto.</param>
    /// <exception cref="InvalidOperationException">Si el producto ya no existe.</exception>
    Task CambiarActivo(int id);

    /// <summary>Elimina un producto de la base de datos.</summary>
    /// <param name="id">Identificador del producto.</param>
    /// <exception cref="InvalidOperationException">Si el producto tiene registros asociados y no se puede borrar.</exception>
    Task Eliminar(int id);
}