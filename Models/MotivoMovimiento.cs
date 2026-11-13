namespace Blazor_Ecommerce.Models;

/// <summary>Razón por la que cambió el stock de un producto.</summary>
public enum MotivoMovimiento
{
    Compra,
    Venta,
    Cancelacion,
    Merma,
    Ajuste
}