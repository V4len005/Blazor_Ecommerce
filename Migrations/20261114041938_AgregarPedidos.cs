using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blazor_Ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class AgregarPedidos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PedidoId",
                table: "MovimientosStock",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    NombreCliente = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DireccionEnvio = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Observaciones = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PedidoItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PedidoId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidoItems_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PedidoItems_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "MovimientosStock",
                keyColumn: "Id",
                keyValue: 1,
                column: "PedidoId",
                value: null);

            migrationBuilder.UpdateData(
                table: "MovimientosStock",
                keyColumn: "Id",
                keyValue: 2,
                column: "PedidoId",
                value: null);

            migrationBuilder.UpdateData(
                table: "MovimientosStock",
                keyColumn: "Id",
                keyValue: 3,
                column: "PedidoId",
                value: null);

            migrationBuilder.UpdateData(
                table: "MovimientosStock",
                keyColumn: "Id",
                keyValue: 4,
                column: "PedidoId",
                value: null);

            migrationBuilder.UpdateData(
                table: "MovimientosStock",
                keyColumn: "Id",
                keyValue: 5,
                column: "PedidoId",
                value: null);

            migrationBuilder.UpdateData(
                table: "MovimientosStock",
                keyColumn: "Id",
                keyValue: 6,
                column: "PedidoId",
                value: null);

            migrationBuilder.UpdateData(
                table: "MovimientosStock",
                keyColumn: "Id",
                keyValue: 7,
                column: "PedidoId",
                value: null);

            migrationBuilder.UpdateData(
                table: "MovimientosStock",
                keyColumn: "Id",
                keyValue: 8,
                column: "PedidoId",
                value: null);

            migrationBuilder.UpdateData(
                table: "MovimientosStock",
                keyColumn: "Id",
                keyValue: 9,
                column: "PedidoId",
                value: null);

            migrationBuilder.UpdateData(
                table: "MovimientosStock",
                keyColumn: "Id",
                keyValue: 10,
                column: "PedidoId",
                value: null);

            migrationBuilder.UpdateData(
                table: "MovimientosStock",
                keyColumn: "Id",
                keyValue: 11,
                column: "PedidoId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_MovimientosStock_PedidoId",
                table: "MovimientosStock",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItems_PedidoId",
                table: "PedidoItems",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItems_ProductoId",
                table: "PedidoItems",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_Estado",
                table: "Pedidos",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_Fecha",
                table: "Pedidos",
                column: "Fecha");

            migrationBuilder.AddForeignKey(
                name: "FK_MovimientosStock_Pedidos_PedidoId",
                table: "MovimientosStock",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovimientosStock_Pedidos_PedidoId",
                table: "MovimientosStock");

            migrationBuilder.DropTable(
                name: "PedidoItems");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_MovimientosStock_PedidoId",
                table: "MovimientosStock");

            migrationBuilder.DropColumn(
                name: "PedidoId",
                table: "MovimientosStock");
        }
    }
}
