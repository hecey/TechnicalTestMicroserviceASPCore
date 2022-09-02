using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTM.Api.Migrations
{
    public partial class UpdateMovimientos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movimiento_cuenta_CuentasId",
                table: "movimiento");

            migrationBuilder.RenameColumn(
                name: "valor",
                table: "movimiento",
                newName: "Valor");

            migrationBuilder.RenameColumn(
                name: "saldo",
                table: "movimiento",
                newName: "Saldo");

            migrationBuilder.RenameColumn(
                name: "CuentasId",
                table: "movimiento",
                newName: "CuentaId");

            migrationBuilder.RenameIndex(
                name: "IX_movimiento_CuentasId",
                table: "movimiento",
                newName: "IX_movimiento_CuentaId");

            migrationBuilder.AddForeignKey(
                name: "FK_movimiento_cuenta_CuentaId",
                table: "movimiento",
                column: "CuentaId",
                principalTable: "cuenta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movimiento_cuenta_CuentaId",
                table: "movimiento");

            migrationBuilder.RenameColumn(
                name: "Valor",
                table: "movimiento",
                newName: "valor");

            migrationBuilder.RenameColumn(
                name: "Saldo",
                table: "movimiento",
                newName: "saldo");

            migrationBuilder.RenameColumn(
                name: "CuentaId",
                table: "movimiento",
                newName: "CuentasId");

            migrationBuilder.RenameIndex(
                name: "IX_movimiento_CuentaId",
                table: "movimiento",
                newName: "IX_movimiento_CuentasId");

            migrationBuilder.AddForeignKey(
                name: "FK_movimiento_cuenta_CuentasId",
                table: "movimiento",
                column: "CuentasId",
                principalTable: "cuenta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
