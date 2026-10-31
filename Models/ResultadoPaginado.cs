namespace Blazor_Ecommerce.Models
{
    /// <summary>Resultado de una consulta paginada: los elementos de la página y los totales.</summary>
    /// <typeparam name="T">Tipo de elemento de la lista.</typeparam>
    public class ResultadoPaginado<T>
    {
        /// <summary>Elementos de la página actual.</summary>
        public List<T> Items { get; set; } = new();

        /// <summary>Cantidad total de registros que cumplen el filtro (todas las páginas).</summary>
        public int TotalRegistros { get; set; }

        /// <summary>Número de la página devuelta (empieza en 1).</summary>
        public int Pagina { get; set; } = 1;

        /// <summary>Cantidad de elementos por página.</summary>
        public int TamanioPagina { get; set; } = 10;

        /// <summary>Cantidad de páginas necesarias para mostrar todos los registros.</summary>
        public int TotalPaginas =>
            TamanioPagina <= 0 ? 0 : (int)Math.Ceiling(TotalRegistros / (double)TamanioPagina);
    }
}
