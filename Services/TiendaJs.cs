using Microsoft.JSInterop;

namespace Blazor_Ecommerce.Services;

/// <summary>
/// Puente hacia el módulo JavaScript wwwroot/js/tienda.js. Importa el módulo una sola vez por
/// conexión y lo libera al terminar. Solo debe usarse después del primer renderizado de un
/// componente interactivo (en OnAfterRenderAsync o en un evento), nunca durante el prerenderizado.
/// </summary>
public class TiendaJs : IAsyncDisposable
{
    private readonly IJSRuntime _js;
    private Task<IJSObjectReference>? _carga;

    public TiendaJs(IJSRuntime js)
    {
        _js = js;
    }

    /// <summary>Lee la preferencia de modo oscuro guardada en el navegador.</summary>
    /// <returns>True si la persona había activado el modo oscuro.</returns>
    public async Task<bool> ObtenerModoOscuro() =>
        await (await Modulo()).InvokeAsync<bool>("obtenerModoOscuro");

    /// <summary>Aplica o quita el modo oscuro en la página, sin guardar la preferencia.</summary>
    /// <param name="activar">True para activar el modo oscuro.</param>
    public async Task AplicarModoOscuro(bool activar) =>
        await (await Modulo()).InvokeVoidAsync("aplicarModoOscuro", activar);

    /// <summary>Aplica el modo oscuro y guarda la preferencia para la próxima visita.</summary>
    /// <param name="activar">True para activar el modo oscuro.</param>
    public async Task EstablecerModoOscuro(bool activar) =>
        await (await Modulo()).InvokeVoidAsync("establecerModoOscuro", activar);

    /// <summary>Copia un texto al portapapeles (con un método alternativo si no hay https).</summary>
    /// <param name="texto">Texto a copiar.</param>
    /// <returns>True si se pudo copiar.</returns>
    public async Task<bool> CopiarAlPortapapeles(string texto) =>
        await (await Modulo()).InvokeAsync<bool>("copiarAlPortapapeles", texto);

    /// <summary>Abre el diálogo de impresión del navegador.</summary>
    public async Task Imprimir() =>
        await (await Modulo()).InvokeVoidAsync("imprimir");

    /// <summary>Lleva el foco a un elemento de la página.</summary>
    /// <param name="id">Atributo id del elemento.</param>
    /// <param name="soloConMouse">Si es true, no lo hace en pantallas táctiles.</param>
    /// <returns>True si se programó el foco.</returns>
    public async Task<bool> EnfocarElemento(string id, bool soloConMouse = true) =>
        await (await Modulo()).InvokeAsync<bool>("enfocarElemento", id, soloConMouse);

    /// <summary>Activa la tecla "/" para llevar el foco al campo indicado.</summary>
    /// <param name="id">Atributo id del campo de búsqueda.</param>
    public async Task RegistrarAtajoBuscador(string id) =>
        await (await Modulo()).InvokeVoidAsync("registrarAtajoBuscador", id);

    /// <summary>Desactiva el atajo de la tecla "/".</summary>
    public async Task QuitarAtajoBuscador() =>
        await (await Modulo()).InvokeVoidAsync("quitarAtajoBuscador");

    /// <summary>Consulta el permiso actual de notificaciones.</summary>
    /// <returns>"granted", "denied", "default" o "no-soportado".</returns>
    public async Task<string> PermisoNotificaciones() =>
        await (await Modulo()).InvokeAsync<string>("permisoNotificaciones");

    /// <summary>Pide permiso para mostrar notificaciones (debe originarse en un clic).</summary>
    /// <returns>"granted", "denied", "default" o "no-soportado".</returns>
    public async Task<string> PedirPermisoNotificaciones() =>
        await (await Modulo()).InvokeAsync<string>("pedirPermisoNotificaciones");

    /// <summary>Muestra una notificación del navegador (si hay permiso).</summary>
    /// <param name="titulo">Título de la notificación.</param>
    /// <param name="mensaje">Texto de la notificación.</param>
    /// <returns>True si se mostró.</returns>
    public async Task<bool> MostrarNotificacion(string titulo, string mensaje) =>
        await (await Modulo()).InvokeAsync<bool>("mostrarNotificacion", titulo, mensaje);

    /// <summary>Guarda un texto en localStorage.</summary>
    /// <param name="clave">Nombre con el que se guarda.</param>
    /// <param name="valor">Texto a guardar.</param>
    /// <returns>True si se pudo guardar.</returns>
    public async Task<bool> GuardarEnLocalStorage(string clave, string valor) =>
        await (await Modulo()).InvokeAsync<bool>("guardarEnLocalStorage", clave, valor);

    /// <summary>Lee un texto de localStorage.</summary>
    /// <param name="clave">Nombre con el que se guardó.</param>
    /// <returns>El texto guardado, o null si no hay nada.</returns>
    public async Task<string?> LeerDeLocalStorage(string clave) =>
        await (await Modulo()).InvokeAsync<string?>("leerDeLocalStorage", clave);

    /// <summary>Borra una clave de localStorage.</summary>
    /// <param name="clave">Nombre de la clave.</param>
    public async Task BorrarDeLocalStorage(string clave) =>
        await (await Modulo()).InvokeVoidAsync("borrarDeLocalStorage", clave);

    // Importa el módulo la primera vez; si esa importación había fallado, se vuelve a intentar.
    private Task<IJSObjectReference> Modulo()
    {
        if (_carga is null || _carga.IsFaulted || _carga.IsCanceled)
        {
            _carga = _js.InvokeAsync<IJSObjectReference>("import", $"./js/tienda.js?v={VersionApp.Valor}").AsTask();
        }

        return _carga;
    }

    public async ValueTask DisposeAsync()
    {
        if (_carga is { IsCompletedSuccessfully: true })
        {
            try
            {
                await (await _carga).DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
                // La conexión ya se cerró: no hay nada que liberar.
            }
        }
    }
}