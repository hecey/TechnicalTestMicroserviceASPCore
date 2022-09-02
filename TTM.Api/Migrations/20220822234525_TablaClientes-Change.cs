using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTM.Api.Migrations
{
    public partial class TablaClientesChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClienteID",
                table: "personas");

            migrationBuilder.RenameColumn(
                name: "ClienteEstado",
                table: "personas",
                newName: "Estado");

            migrationBuilder.RenameColumn(
                name: "ClienteContrasena",
                table: "personas",
                newName: "Contrasena");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Estado",
                table: "personas",
                newName: "ClienteEstado");

            migrationBuilder.RenameColumn(
                name: "Contrasena",
                table: "personas",
                newName: "ClienteContrasena");

            migrationBuilder.AddColumn<int>(
                name: "ClienteID",
                table: "personas",
                type: "int",
                nullable: true);
        }
    }
}
