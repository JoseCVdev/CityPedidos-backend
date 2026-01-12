using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityPedidos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    nIdCliente = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vNumeroDocumento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vNombres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vApellidoPaterno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vApellidoMaterno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bitEstado = table.Column<bool>(type: "bit", nullable: false),
                    nIdUsuarioCrea = table.Column<long>(type: "bigint", nullable: false),
                    dtFechaCrea = table.Column<DateTime>(type: "datetime2", nullable: false),
                    nIdUsuarioMod = table.Column<long>(type: "bigint", nullable: true),
                    dtFechaMod = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ndUsuarioEli = table.Column<long>(type: "bigint", nullable: true),
                    dtFechaEli = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.nIdCliente);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    nIdMenu = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vNombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vIcono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bitEstado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.nIdMenu);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    nIdRol = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vNombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vDescripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bitEstado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.nIdRol);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    nIdPedido = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vNumeroPedido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dtFechaPedido = table.Column<DateTime>(type: "datetime2", nullable: false),
                    decTotal = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    bitEstado = table.Column<bool>(type: "bit", nullable: false),
                    nIdCliente = table.Column<long>(type: "bigint", nullable: false),
                    nIdUsuarioCrea = table.Column<long>(type: "bigint", nullable: false),
                    dtFechaCrea = table.Column<DateTime>(type: "datetime2", nullable: false),
                    nIdUsuarioMod = table.Column<long>(type: "bigint", nullable: true),
                    dtFechaMod = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ndUsuarioEli = table.Column<long>(type: "bigint", nullable: true),
                    dtFechaEli = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.nIdPedido);
                    table.ForeignKey(
                        name: "FK_Pedidos_Clientes_nIdCliente",
                        column: x => x.nIdCliente,
                        principalTable: "Clientes",
                        principalColumn: "nIdCliente",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolMenus",
                columns: table => new
                {
                    NIdRol = table.Column<long>(type: "bigint", nullable: false),
                    nIdMenu = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolMenus", x => new { x.NIdRol, x.nIdMenu });
                    table.ForeignKey(
                        name: "FK_RolMenus_Menus_nIdMenu",
                        column: x => x.nIdMenu,
                        principalTable: "Menus",
                        principalColumn: "nIdMenu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolMenus_Roles_NIdRol",
                        column: x => x.NIdRol,
                        principalTable: "Roles",
                        principalColumn: "nIdRol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    nIdUsuario = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vCorreo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vContrasena = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    vNombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bitEstado = table.Column<bool>(type: "bit", nullable: false),
                    nIdRol = table.Column<long>(type: "bigint", nullable: false),
                    nIdUsuarioCrea = table.Column<long>(type: "bigint", nullable: false),
                    dtFechaCrea = table.Column<DateTime>(type: "datetime2", nullable: false),
                    nIdUsuarioMod = table.Column<long>(type: "bigint", nullable: true),
                    dtFechaMod = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ndUsuarioEli = table.Column<long>(type: "bigint", nullable: true),
                    dtFechaEli = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.nIdUsuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_nIdRol",
                        column: x => x.nIdRol,
                        principalTable: "Roles",
                        principalColumn: "nIdRol",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_nIdCliente",
                table: "Pedidos",
                column: "nIdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_RolMenus_nIdMenu",
                table: "RolMenus",
                column: "nIdMenu");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_nIdRol",
                table: "Usuarios",
                column: "nIdRol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "RolMenus");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
