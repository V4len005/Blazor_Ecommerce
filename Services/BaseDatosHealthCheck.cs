using Blazor_Ecommerce.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Blazor_Ecommerce.Services;

/// <summary>Comprueba que la aplicación pueda conectarse a la base de datos.</summary>
public class BaseDatosHealthCheck : IHealthCheck
{
    private readonly IDbContextFactory<TiendaDbContext> _fabrica;
    private readonly ILogger<BaseDatosHealthCheck> _logger;

    public BaseDatosHealthCheck(IDbContextFactory<TiendaDbContext> fabrica, ILogger<BaseDatosHealthCheck> logger)
    {
        _fabrica = fabrica;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var contexto = await _fabrica.CreateDbContextAsync(cancellationToken);

            return await contexto.Database.CanConnectAsync(cancellationToken)
                ? HealthCheckResult.Healthy("La base de datos responde.")
                : HealthCheckResult.Unhealthy("No se pudo conectar a la base de datos.");
        }
        catch (Exception ex)
        {
            // El detalle técnico va al log; la respuesta pública no lo incluye.
            _logger.LogError(ex, "Falló la verificación de la base de datos.");

            return HealthCheckResult.Unhealthy("La base de datos no responde.");
        }
    }
}