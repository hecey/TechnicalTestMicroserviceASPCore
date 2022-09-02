using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TTM.Api.Migrations
{
    public partial class TablaClientesAdicionada : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClienteContrasena",
                table: "personas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClienteEstado",
                table: "personas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClienteID",
                table: "personas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "personas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClienteContrasena",
                table: "personas");

            migrationBuilder.DropColumn(
                name: "ClienteEstado",
                table: "personas");

            migrationBuilder.DropColumn(
                name: "ClienteID",
                table: "personas");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "personas");
        }
    }
}
