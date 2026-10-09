using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Blazor_Ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class CreacionInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Sku = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Especificaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    StockActual = table.Column<int>(type: "int", nullable: false),
                    StockMinimo = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    ImagenUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Productos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Consolas de videojuegos de sobremesa y portátiles", "Consolas" },
                    { 2, "Computadoras portátiles para estudio, trabajo y juegos", "Notebooks" },
                    { 3, "Periféricos de entrada para la computadora", "Teclados y Mouse" },
                    { 4, "Auriculares con cable e inalámbricos", "Auriculares" },
                    { 5, "Pantallas para computadora", "Monitores" },
                    { 6, "Hubs, mochilas y otros complementos", "Accesorios" }
                });

            migrationBuilder.InsertData(
                table: "Productos",
                columns: new[] { "Id", "Activo", "CategoriaId", "Descripcion", "Especificaciones", "ImagenUrl", "Marca", "Nombre", "Precio", "Sku", "StockActual", "StockMinimo" },
                values: new object[,]
                {
                    { 1, true, 1, "Consola de videojuegos para jugar en el televisor.", "1 TB SSD / salida 4K / 1 control incluido", null, "Nexa", "Consola de sobremesa 1 TB", 1100000m, "CON-0001", 15, 3 },
                    { 2, true, 1, "Consola portátil para jugar en cualquier lugar.", "512 GB / pantalla de 7 pulgadas", null, "Nexa", "Consola portátil 512 GB", 750000m, "CON-0002", 8, 2 },
                    { 3, true, 2, "Notebook para estudio y trabajo diario.", "16 GB RAM / 512 GB SSD", null, "Orbit", "Notebook 15,6 pulgadas Ryzen 5", 1350000m, "NOT-0001", 10, 2 },
                    { 4, true, 2, "Notebook potente para videojuegos y edición.", "32 GB RAM / 1 TB SSD / placa de video dedicada", null, "Orbit", "Notebook gamer 16 pulgadas", 2400000m, "NOT-0002", 0, 2 },
                    { 5, true, 3, "Teclado mecánico compacto sin bloque numérico.", "Switches rojos / retroiluminación RGB", null, "Vertex", "Teclado mecánico TKL", 95000m, "TEC-0001", 25, 5 },
                    { 6, true, 3, "Mouse cómodo para largas jornadas de trabajo.", "2400 DPI / Bluetooth y 2,4 GHz", null, "Vertex", "Mouse inalámbrico ergonómico", 38000m, "MOU-0001", 40, 10 },
                    { 7, true, 4, "Auriculares plegables para música y llamadas.", "Bluetooth 5.0 / 30 horas de batería", null, "Pulso", "Auriculares inalámbricos con micrófono", 72000m, "AUR-0001", 2, 5 },
                    { 8, true, 4, "Auriculares con sonido envolvente para juegos.", "USB / sonido 7.1 / micrófono desmontable", null, "Pulso", "Auriculares gamer 7.1", 64000m, "AUR-0002", 18, 5 },
                    { 9, true, 5, "Monitor para oficina y estudio.", "1920x1080 / 75 Hz / panel IPS", null, "Kodo", "Monitor 24 pulgadas Full HD", 260000m, "MON-0001", 12, 3 },
                    { 10, true, 5, "Monitor curvo con alta tasa de refresco para juegos.", "2560x1440 / 144 Hz / panel VA curvo", null, "Kodo", "Monitor curvo 27 pulgadas 144 Hz", 480000m, "MON-0002", 6, 2 },
                    { 11, true, 6, "Adaptador para ampliar los puertos de la notebook.", "HDMI / 3 USB-A / lector SD / carga rápida", null, "Pulso", "Hub USB-C 7 en 1", 45000m, "ACC-0001", 30, 8 },
                    { 12, false, 6, "Mochila con compartimento acolchado para notebook.", "Impermeable / compartimento acolchado", null, "Orbit", "Mochila para notebook 15,6 pulgadas", 52000m, "ACC-0002", 20, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Nombre",
                table: "Categorias",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CategoriaId",
                table: "Productos",
                column: "CategoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Categorias");
        }
    }
}
