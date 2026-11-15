using Blazor_Ecommerce.Components;
using Blazor_Ecommerce.Data;
using Blazor_Ecommerce.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// La cadena de conexión nunca está en el código ni en los appsettings:
// en desarrollo sale de User Secrets y en producción de la variable de entorno ConnectionStrings__Default.
var cadenaConexion = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException(
        "Falta la cadena de conexión 'Default'. En desarrollo configurala con User Secrets; " +
        "en producción, con la variable de entorno ConnectionStrings__Default.");

// Servicios de Blazor con modo interactivo de servidor.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// EF Core: fábrica de contextos de corta vida (recomendado en Blazor Server).
builder.Services.AddDbContextFactory<TiendaDbContext>(opciones =>
    opciones.UseSqlServer(cadenaConexion));

// Servicios de datos: una instancia por circuito (conexión del usuario).
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IEstadisticaService, EstadisticaService>();

// Estado del carrito: uno por visitante, con aviso de cambios.
builder.Services.AddScoped<CarritoState>();

// Puente hacia el módulo JavaScript: se importa una vez por visitante y se libera al terminar.
builder.Services.AddScoped<TiendaJs>();

// Chequeo de salud: responde en /health si la base de datos contesta.
builder.Services.AddHealthChecks()
    .AddCheck<BaseDatosHealthCheck>("base-de-datos");

var app = builder.Build();

// Al iniciar, crea la base si no existe y aplica las migraciones pendientes.
var fabrica = app.Services.GetRequiredService<IDbContextFactory<TiendaDbContext>>();

try
{
    await using var contexto = await fabrica.CreateDbContextAsync();

    app.Logger.LogInformation("Aplicando las migraciones pendientes de la base de datos...");
    await contexto.Database.MigrateAsync();
    app.Logger.LogInformation("Base de datos lista.");
}
catch (Exception ex)
{
    app.Logger.LogCritical(ex, "No se pudo preparar la base de datos al iniciar la aplicación.");
    throw;
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapHealthChecks("/health");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();