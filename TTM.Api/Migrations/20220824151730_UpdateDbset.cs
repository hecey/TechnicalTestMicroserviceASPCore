using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTM.Api.Migrations
{
    public partial class UpdateDbset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cuenta_persona_ClienteId",
                table: "cuenta");

            migrationBuilder.DropForeignKey(
                name: "FK_movimiento_cuenta_CuentaId",
                table: "movimiento");

            migrationBuilder.DropPrimaryKey(
                name: "PK_persona",
                table: "persona");

            migrationBuilder.DropPrimaryKey(
                name: "PK_movimiento",
                table: "movimiento");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cuenta",
                table: "cuenta");

            migrationBuilder.RenameTable(
                name: "persona",
                newName: "Persona");

            migrationBuilder.RenameTable(
                name: "movimiento",
                newName: "Movimiento");

            migrationBuilder.RenameTable(
                name: "cuenta",
                newName: "Cuenta");

            migrationBuilder.RenameIndex(
                name: "IX_movimiento_CuentaId",
                table: "Movimiento",
                newName: "IX_Movimiento_CuentaId");

            migrationBuilder.RenameIndex(
                name: "IX_cuenta_ClienteId",
                table: "Cuenta",
                newName: "IX_Cuenta_ClienteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Persona",
                table: "Persona",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movimiento",
                table: "Movimiento",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cuenta",
                table: "Cuenta",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cuenta_Persona_ClienteId",
                table: "Cuenta",
                column: "ClienteId",
                principalTable: "Persona",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Movimiento_Cuenta_CuentaId",
                table: "Movimiento",
                column: "CuentaId",
                principalTable: "Cuenta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cuenta_Persona_ClienteId",
                table: "Cuenta");

            migrationBuilder.DropForeignKey(
                name: "FK_Movimiento_Cuenta_CuentaId",
                table: "Movimiento");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Persona",
                table: "Persona");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movimiento",
                table: "Movimiento");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cuenta",
                table: "Cuenta");

            migrationBuilder.RenameTable(
                name: "Persona",
                newName: "persona");

            migrationBuilder.RenameTable(
                name: "Movimiento",
                newName: "movimiento");

            migrationBuilder.RenameTable(
                name: "Cuenta",
                newName: "cuenta");

            migrationBuilder.RenameIndex(
                name: "IX_Movimiento_CuentaId",
                table: "movimiento",
                newName: "IX_movimiento_CuentaId");

            migrationBuilder.RenameIndex(
                name: "IX_Cuenta_ClienteId",
                table: "cuenta",
                newName: "IX_cuenta_ClienteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_persona",
                table: "persona",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_movimiento",
                table: "movimiento",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cuenta",
                table: "cuenta",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_cuenta_persona_ClienteId",
                table: "cuenta",
                column: "ClienteId",
                principalTable: "persona",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_movimiento_cuenta_CuentaId",
                table: "movimiento",
                column: "CuentaId",
                principalTable: "cuenta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
