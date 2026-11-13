namespace Blazor_Ecommerce.Models;

/// <summary>Operaciones que una persona puede registrar manualmente desde el formulario de stock.</summary>
public enum OperacionStock
{
    /// <summary>Ingreso de mercadería por una compra: suma unidades.</summary>
    Entrada,

    /// <summary>Pérdida de unidades (rotura, vencimiento, robo): resta unidades.</summary>
    Merma,

    /// <summary>Corrección del stock según un conteo físico real.</summary>
    Ajuste
}