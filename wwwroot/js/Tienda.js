const CLAVE_MODO_OSCURO = 'tienda-modo-oscuro';

// Lee la preferencia guardada. Si no hay nada (o el navegador bloquea el acceso), devuelve false.
export function obtenerModoOscuro() {
    try {
        return localStorage.getItem(CLAVE_MODO_OSCURO) === 'true';
    } catch {
        return false;
    }
}

// Agrega o quita la clase dark-mode del <body> (no guarda nada).
export function aplicarModoOscuro(activar) {
    document.body.classList.toggle('dark-mode', activar);
}

// Aplica el modo y además lo guarda para la próxima visita.
export function establecerModoOscuro(activar) {
    aplicarModoOscuro(activar);
    try {
        localStorage.setItem(CLAVE_MODO_OSCURO, activar ? 'true' : 'false');
    } catch {
        // Si no se puede guardar, el modo igual queda aplicado en esta sesión.
    }
}