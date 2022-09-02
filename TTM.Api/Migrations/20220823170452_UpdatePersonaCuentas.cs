using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTM.Api.Migrations
{
    public partial class UpdatePersonaCuentas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cuenta_persona_ClientesId",
                table: "cuenta");

            migrationBuilder.RenameColumn(
                name: "ClientesId",
                table: "cuenta",
                newName: "ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_cuenta_ClientesId",
                table: "cuenta",
                newName: "IX_cuenta_ClienteId");

            migrationBuilder.AlterColumn<string>(
                name: "Identificacion",
                table: "persona",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_cuenta_persona_ClienteId",
                table: "cuenta",
                column: "ClienteId",
                principalTable: "persona",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cuenta_persona_ClienteId",
                table: "cuenta");

            migrationBuilder.RenameColumn(
                name: "ClienteId",
                table: "cuenta",
                newName: "ClientesId");

            migrationBuilder.RenameIndex(
                name: "IX_cuenta_ClienteId",
                table: "cuenta",
                newName: "IX_cuenta_ClientesId");

            migrationBuilder.AlterColumn<string>(
                name: "Identificacion",
                table: "persona",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_cuenta_persona_ClientesId",
                table: "cuenta",
                column: "ClientesId",
                principalTable: "persona",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
