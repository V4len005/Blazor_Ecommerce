const CLAVE_MODO_OSCURO = 'tienda-modo-oscuro';

// ===== Modo oscuro =====

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

// ===== Almacenamiento local (lo usa el carrito) =====

export function guardarEnLocalStorage(clave, valor) {
    try {
        localStorage.setItem(clave, valor);
        return true;
    } catch {
        return false;
    }
}

export function leerDeLocalStorage(clave) {
    try {
        return localStorage.getItem(clave);
    } catch {
        return null;
    }
}

export function borrarDeLocalStorage(clave) {
    try {
        localStorage.removeItem(clave);
    } catch {
        // Si el navegador lo bloquea, no hay nada que borrar.
    }
}

// ===== Portapapeles =====

export async function copiarAlPortapapeles(texto) {
    try {
        if (navigator.clipboard && window.isSecureContext) {
            await navigator.clipboard.writeText(texto);
            return true;
        }
    } catch {
        // Si falla, se prueba el método alternativo.
    }

    return copiarConTextarea(texto);
}

// Plan B para conexiones sin https: se copia desde un campo de texto oculto.
function copiarConTextarea(texto) {
    const area = document.createElement('textarea');
    area.value = texto;
    area.setAttribute('readonly', '');
    area.style.position = 'fixed';
    area.style.opacity = '0';
    document.body.appendChild(area);
    area.select();

    let copiado = false;
    try {
        copiado = document.execCommand('copy');
    } catch {
        copiado = false;
    }

    document.body.removeChild(area);
    return copiado;
}

// ===== Impresión =====

export function imprimir() {
    window.print();
}

// ===== Foco =====

// Enfoca un elemento por su id. Con soloConMouse, no lo hace en pantallas táctiles
// (para no abrir el teclado del celular sin que la persona lo pida).
export function enfocarElemento(id, soloConMouse = true) {
    if (soloConMouse && window.matchMedia('(pointer: coarse)').matches) {
        return false;
    }

    // Blazor mueve el foco al <h1> al navegar; se espera un instante para que el foco final quede en este campo.
    setTimeout(() => document.getElementById(id)?.focus(), 150);
    return true;
}

// Atajo de teclado: la tecla "/" lleva el foco al campo indicado (si la persona no está escribiendo en otro).
let manejadorAtajo = null;

export function registrarAtajoBuscador(id) {
    quitarAtajoBuscador();

    manejadorAtajo = (evento) => {
        if (evento.key !== '/' || evento.ctrlKey || evento.metaKey || evento.altKey) {
            return;
        }

        const activo = document.activeElement;
        const escribiendo = activo &&
            (['INPUT', 'TEXTAREA', 'SELECT'].includes(activo.tagName) || activo.isContentEditable);

        if (escribiendo) {
            return;
        }

        const campo = document.getElementById(id);

        if (campo) {
            evento.preventDefault();
            campo.focus();
            campo.select();
        }
    };

    document.addEventListener('keydown', manejadorAtajo);
}

export function quitarAtajoBuscador() {
    if (manejadorAtajo) {
        document.removeEventListener('keydown', manejadorAtajo);
        manejadorAtajo = null;
    }
}

// ===== Notificaciones del navegador =====

export function permisoNotificaciones() {
    return 'Notification' in window ? Notification.permission : 'no-soportado';
}

// Pide permiso (si todavía no se decidió). Debe llamarse a partir de un clic de la persona.
export async function pedirPermisoNotificaciones() {
    if (!('Notification' in window)) {
        return 'no-soportado';
    }

    if (Notification.permission === 'default') {
        try {
            return await Notification.requestPermission();
        } catch {
            return Notification.permission;
        }
    }

    return Notification.permission;
}

export function mostrarNotificacion(titulo, mensaje) {
    if (!('Notification' in window) || Notification.permission !== 'granted') {
        return false;
    }

    try {
        new Notification(titulo, { body: mensaje, tag: 'valtech-stock-bajo' });
        return true;
    } catch {
        return false;
    }
}