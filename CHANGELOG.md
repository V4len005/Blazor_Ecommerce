# Changelog

Todos los cambios importantes de este proyecto se documentan en este archivo.

El formato se basa en [Keep a Changelog](https://keepachangelog.com/es-ES/1.1.0/) y el proyecto sigue el [Versionado Semántico](https://semver.org/lang/es/).

## [Sin publicar]

### Agregado
- Configuración por ambiente (`appsettings.Development.json` y `appsettings.Production.json`), sin credenciales.
- Registro de operaciones con `ILogger<T>` en los servicios, sin datos personales del cliente.
- Endpoint `/health` con un chequeo propio de la base de datos.
- Validación de la cadena de conexión al iniciar, con un mensaje de error claro.
- README con diagramas Mermaid de arquitectura, modelo de datos y despliegue.
- Documentación XML generada por el compilador.

### Cambiado
- El nombre de la tienda se define en un solo lugar (`Tienda.Nombre`).
- El formato de precios y el módulo de JavaScript se reutilizan en lugar de repetirse.

## [0.3.1] - 2026-11-14

### Corregido
- El carrito y el checkout ahora se actualizan cuando el carrito se restaura al recargar la página.
- Los estilos y el JavaScript llevan una versión en la dirección, para que el navegador no use copias viejas después de cada despliegue.

## [0.3.0] - 2026-11-14

### Agregado
- Administración de pedidos: lista paginada con filtros, detalle y cambio de estado con reglas de transición.
- Cancelación de pedidos con devolución del stock y registro del movimiento.
- Estadísticas reales en el panel: productos, stock bajo, pedidos del día, total vendido, más vendidos y pedidos por estado.
- Funciones con JavaScript Interop: carrito persistente en `localStorage`, copiar el resumen del pedido, imprimir el comprobante, foco automático en buscadores, atajo de teclado y notificación del navegador por stock bajo.
- Componente `CascadingValue` para compartir el pedido entre componentes.

## [0.2.0] - 2026-11-14

### Agregado
- Catálogo público con búsqueda, filtro por categoría y paginación contra la base de datos.
- Detalle de producto y portada con categorías y últimos productos.
- Carrito de compras con `CarritoState` (State Service) y contador reactivo en el encabezado.
- Checkout con validación avanzada y confirmación del pedido en una sola transacción.
- Entidades de pedido y movimiento de stock, con migraciones y control de concurrencia.
- Gestión de stock: entradas, mermas y ajustes con historial paginado y alertas de stock bajo.

## [0.1.0] - 2026-11-13

### Agregado
- Proyecto base con Blazor Web App en modo servidor (.NET 8) y Entity Framework Core con SQL Server.
- Layouts semánticos de tienda y de administración, modo oscuro persistente y estilos con CSS Isolation y Bootstrap 5.
- Entidades de categoría y producto, migración inicial y datos de ejemplo.
- CRUD de productos con búsqueda, paginación, validación avanzada y modal de confirmación.
- Flujo de trabajo con Git Flow, Conventional Commits y un workflow de GitHub Actions con despliegue automático en MonsterASP.NET.