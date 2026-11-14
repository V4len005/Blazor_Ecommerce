using Blazor_Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Blazor_Ecommerce.Data;

/// <summary>Contexto de EF Core: representa la sesión de trabajo con la base de datos de la tienda.</summary>
public class TiendaDbContext : DbContext
{
    public TiendaDbContext(DbContextOptions<TiendaDbContext> opciones)
        : base(opciones)
    {
    }

    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<MovimientoStock> MovimientosStock => Set<MovimientoStock>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<PedidoItem> PedidoItems => Set<PedidoItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entidad =>
        {
            // No puede haber dos categorías con el mismo nombre.
            entidad.HasIndex(c => c.Nombre).IsUnique();
            entidad.HasData(SemillaDatos.Categorias);
        });

        modelBuilder.Entity<Producto>(entidad =>
        {
            // Dinero: 18 dígitos en total, 2 decimales.
            entidad.Property(p => p.Precio).HasPrecision(18, 2);

            // Si el stock cambió desde que se leyó, el guardado falla en vez de pisarlo.
            entidad.Property(p => p.StockActual).IsConcurrencyToken();

            // Una categoría tiene muchos productos; un producto pertenece a una categoría.
            entidad.HasOne(p => p.Categoria)
                   .WithMany(c => c.Productos)
                   .HasForeignKey(p => p.CategoriaId)
                   .OnDelete(DeleteBehavior.Restrict);

            entidad.HasData(SemillaDatos.Productos);
        });

        modelBuilder.Entity<MovimientoStock>(entidad =>
        {
            // Los enums se guardan como texto, no como número.
            entidad.Property(m => m.Tipo).HasConversion<string>().HasMaxLength(20);
            entidad.Property(m => m.Motivo).HasConversion<string>().HasMaxLength(20);

            // Un producto tiene muchos movimientos; no se puede borrar un producto con historial.
            entidad.HasOne(m => m.Producto)
                   .WithMany()
                   .HasForeignKey(m => m.ProductoId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Un movimiento puede venir de un pedido (ventas y cancelaciones); los manuales no.
            entidad.HasOne(m => m.Pedido)
                   .WithMany()
                   .HasForeignKey(m => m.PedidoId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Acelera la consulta del historial de un producto ordenado por fecha.
            entidad.HasIndex(m => new { m.ProductoId, m.Fecha });

            entidad.HasData(SemillaMovimientos.Movimientos);
        });

        modelBuilder.Entity<Pedido>(entidad =>
        {
            // Si el estado cambió desde que se leyó el pedido, el guardado falla en vez de pisarlo.
            entidad.Property(p => p.Estado)
                   .HasConversion<string>()
                   .HasMaxLength(20)
                   .IsConcurrencyToken();

            entidad.Property(p => p.Total).HasPrecision(18, 2);

            // Aceleran la lista de pedidos por fecha y el filtro por estado.
            entidad.HasIndex(p => p.Fecha);
            entidad.HasIndex(p => p.Estado);
        });

        modelBuilder.Entity<PedidoItem>(entidad =>
        {
            entidad.Property(i => i.PrecioUnitario).HasPrecision(18, 2);

            // Un pedido tiene muchos ítems; si se borrara el pedido, se borran sus ítems.
            entidad.HasOne(i => i.Pedido)
                   .WithMany(p => p.Items)
                   .HasForeignKey(i => i.PedidoId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Un producto con pedidos no se puede borrar: se desactiva.
            entidad.HasOne(i => i.Producto)
                   .WithMany()
                   .HasForeignKey(i => i.ProductoId)
                   .OnDelete(DeleteBehavior.Restrict);
        });
    }
}