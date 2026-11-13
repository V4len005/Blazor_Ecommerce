namespace Blazor_Ecommerce.Models;

/// <summary>Indica cómo afecta un movimiento al stock del producto.</summary>
public enum TipoMovimiento
{
    /// <summary>Suma unidades al stock.</summary>
    Entrada,

    /// <summary>Resta unidades al stock.</summary>
    Salida,

    /// <summary>Corrige el stock; la cantidad lleva signo (positiva suma, negativa resta).</summary>
    Ajuste
}