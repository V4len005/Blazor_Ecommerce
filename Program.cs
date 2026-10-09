using Blazor_Ecommerce.Components;
using Blazor_Ecommerce.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Servicios de Blazor con modo interactivo de servidor.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// EF Core: fábrica de contextos de corta vida (recomendado en Blazor Server).
builder.Services.AddDbContextFactory<TiendaDbContext>(opciones =>
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

// Al iniciar, crea la base si no existe y aplica las migraciones pendientes.
var fabrica = app.Services.GetRequiredService<IDbContextFactory<TiendaDbContext>>();
await using (var contexto = await fabrica.CreateDbContextAsync())
{
    await contexto.Database.MigrateAsync();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();