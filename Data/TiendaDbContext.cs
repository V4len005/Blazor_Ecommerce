using Microsoft.EntityFrameworkCore;
using Blazor_Ecommerce.Models;

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

            // Una categoría tiene muchos productos; un producto pertenece a una categoría.
            entidad.HasOne(p => p.Categoria)
                   .WithMany(c => c.Productos)
                   .HasForeignKey(p => p.CategoriaId)
                   .OnDelete(DeleteBehavior.Restrict);

            entidad.HasData(SemillaDatos.Productos);
        });
    }
}