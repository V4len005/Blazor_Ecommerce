using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Blazor_Ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class AgregarMovimientoStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovimientosStock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientosStock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovimientosStock_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "MovimientosStock",
                columns: new[] { "Id", "Cantidad", "Fecha", "Motivo", "Observacion", "ProductoId", "Tipo" },
                values: new object[,]
                {
                    { 1, 15, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ajuste", "Stock inicial", 1, "Ajuste" },
                    { 2, 8, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ajuste", "Stock inicial", 2, "Ajuste" },
                    { 3, 10, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ajuste", "Stock inicial", 3, "Ajuste" },
                    { 4, 25, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ajuste", "Stock inicial", 5, "Ajuste" },
                    { 5, 40, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ajuste", "Stock inicial", 6, "Ajuste" },
                    { 6, 2, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ajuste", "Stock inicial", 7, "Ajuste" },
                    { 7, 18, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ajuste", "Stock inicial", 8, "Ajuste" },
                    { 8, 12, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ajuste", "Stock inicial", 9, "Ajuste" },
                    { 9, 6, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ajuste", "Stock inicial", 10, "Ajuste" },
                    { 10, 30, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ajuste", "Stock inicial", 11, "Ajuste" },
                    { 11, 20, new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ajuste", "Stock inicial", 12, "Ajuste" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosStock_ProductoId_Fecha",
                table: "MovimientosStock",
                columns: new[] { "ProductoId", "Fecha" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovimientosStock");
        }
    }
}
