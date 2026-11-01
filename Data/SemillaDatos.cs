using Blazor_Ecommerce.Models;


namespace Blazor_Ecommerce.Data;

/// <summary>
/// Datos iniciales de la tienda. Los nombres, marcas y precios son FICTICIOS,
/// inventados solo para la demostración del proyecto.
/// </summary>
public static class SemillaDatos
{
    public static readonly Categoria[] Categorias =
    [
        new Categoria { Id = 1, Nombre = "Consolas", Descripcion = "Consolas de videojuegos de sobremesa y portátiles" },
        new Categoria { Id = 2, Nombre = "Notebooks", Descripcion = "Computadoras portátiles para estudio, trabajo y juegos" },
        new Categoria { Id = 3, Nombre = "Teclados y Mouse", Descripcion = "Periféricos de entrada para la computadora" },
        new Categoria { Id = 4, Nombre = "Auriculares", Descripcion = "Auriculares con cable e inalámbricos" },
        new Categoria { Id = 5, Nombre = "Monitores", Descripcion = "Pantallas para computadora" },
        new Categoria { Id = 6, Nombre = "Accesorios", Descripcion = "Hubs, mochilas y otros complementos" }
    ];

    public static readonly Producto[] Productos =
    [
        new Producto { Id = 1, Nombre = "Consola de sobremesa 1 TB", Marca = "Nexa", Sku = "CON-0001",
            Descripcion = "Consola de videojuegos para jugar en el televisor.",
            Especificaciones = "1 TB SSD / salida 4K / 1 control incluido",
            Precio = 1100000m, StockActual = 15, StockMinimo = 3, Activo = true, CategoriaId = 1 },

        new Producto { Id = 2, Nombre = "Consola portátil 512 GB", Marca = "Nexa", Sku = "CON-0002",
            Descripcion = "Consola portátil para jugar en cualquier lugar.",
            Especificaciones = "512 GB / pantalla de 7 pulgadas",
            Precio = 750000m, StockActual = 8, StockMinimo = 2, Activo = true, CategoriaId = 1 },

        new Producto { Id = 3, Nombre = "Notebook 15,6 pulgadas Ryzen 5", Marca = "Orbit", Sku = "NOT-0001",
            Descripcion = "Notebook para estudio y trabajo diario.",
            Especificaciones = "16 GB RAM / 512 GB SSD",
            Precio = 1350000m, StockActual = 10, StockMinimo = 2, Activo = true, CategoriaId = 2 },

        // Sin stock a propósito: sirve para probar la marca "Sin stock" del catálogo.
        new Producto { Id = 4, Nombre = "Notebook gamer 16 pulgadas", Marca = "Orbit", Sku = "NOT-0002",
            Descripcion = "Notebook potente para videojuegos y edición.",
            Especificaciones = "32 GB RAM / 1 TB SSD / placa de video dedicada",
            Precio = 2400000m, StockActual = 0, StockMinimo = 2, Activo = true, CategoriaId = 2 },

        new Producto { Id = 5, Nombre = "Teclado mecánico TKL", Marca = "Vertex", Sku = "TEC-0001",
            Descripcion = "Teclado mecánico compacto sin bloque numérico.",
            Especificaciones = "Switches rojos / retroiluminación RGB",
            Precio = 95000m, StockActual = 25, StockMinimo = 5, Activo = true, CategoriaId = 3 },

        new Producto { Id = 6, Nombre = "Mouse inalámbrico ergonómico", Marca = "Vertex", Sku = "MOU-0001",
            Descripcion = "Mouse cómodo para largas jornadas de trabajo.",
            Especificaciones = "2400 DPI / Bluetooth y 2,4 GHz",
            Precio = 38000m, StockActual = 40, StockMinimo = 10, Activo = true, CategoriaId = 3 },

        // Stock por debajo del mínimo a propósito: sirve para probar la alerta de stock bajo.
        new Producto { Id = 7, Nombre = "Auriculares inalámbricos con micrófono", Marca = "Pulso", Sku = "AUR-0001",
            Descripcion = "Auriculares plegables para música y llamadas.",
            Especificaciones = "Bluetooth 5.0 / 30 horas de batería",
            Precio = 72000m, StockActual = 2, StockMinimo = 5, Activo = true, CategoriaId = 4 },

        new Producto { Id = 8, Nombre = "Auriculares gamer 7.1", Marca = "Pulso", Sku = "AUR-0002",
            Descripcion = "Auriculares con sonido envolvente para juegos.",
            Especificaciones = "USB / sonido 7.1 / micrófono desmontable",
            Precio = 64000m, StockActual = 18, StockMinimo = 5, Activo = true, CategoriaId = 4 },

        new Producto { Id = 9, Nombre = "Monitor 24 pulgadas Full HD", Marca = "Kodo", Sku = "MON-0001",
            Descripcion = "Monitor para oficina y estudio.",
            Especificaciones = "1920x1080 / 75 Hz / panel IPS",
            Precio = 260000m, StockActual = 12, StockMinimo = 3, Activo = true, CategoriaId = 5 },

        new Producto { Id = 10, Nombre = "Monitor curvo 27 pulgadas 144 Hz", Marca = "Kodo", Sku = "MON-0002",
            Descripcion = "Monitor curvo con alta tasa de refresco para juegos.",
            Especificaciones = "2560x1440 / 144 Hz / panel VA curvo",
            Precio = 480000m, StockActual = 6, StockMinimo = 2, Activo = true, CategoriaId = 5 },

        new Producto { Id = 11, Nombre = "Hub USB-C 7 en 1", Marca = "Pulso", Sku = "ACC-0001",
            Descripcion = "Adaptador para ampliar los puertos de la notebook.",
            Especificaciones = "HDMI / 3 USB-A / lector SD / carga rápida",
            Precio = 45000m, StockActual = 30, StockMinimo = 8, Activo = true, CategoriaId = 6 },

        // Inactivo a propósito: sirve para probar que el catálogo público no lo muestra.
        new Producto { Id = 12, Nombre = "Mochila para notebook 15,6 pulgadas", Marca = "Orbit", Sku = "ACC-0002",
            Descripcion = "Mochila con compartimento acolchado para notebook.",
            Especificaciones = "Impermeable / compartimento acolchado",
            Precio = 52000m, StockActual = 20, StockMinimo = 5, Activo = false, CategoriaId = 6 }
    ];
}