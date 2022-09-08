using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransactionService.Migrations
{
    public partial class CreateInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /* migrationBuilder.CreateTable(
                 name: "Client",
                 columns: table => new
                 {
                     Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                     Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                     Status = table.Column<bool>(type: "bit", nullable: false),
                     Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                     Genre = table.Column<string>(type: "nvarchar(1)", nullable: false),
                     Age = table.Column<int>(type: "int", nullable: false),
                     Identification = table.Column<string>(type: "nvarchar(max)", nullable: false),
                     Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                     Phone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                 },
                 constraints: table =>
                 {
                     table.PrimaryKey("PK_Client", x => x.Id);
                 });
            */
            /*
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InitialBalance = table.Column<decimal>(type: "decimal(14,2)", precision: 14, scale: 2, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });*/

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transaction_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            /* migrationBuilder.CreateIndex(
                 name: "IX_Account_ClientId",
                 table: "Account",
                 column: "ClientId");*/

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_AccountId",
                table: "Transaction",
                column: "AccountId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transaction");
            /*
                        migrationBuilder.DropTable(
                            name: "Account");

                         migrationBuilder.DropTable(
                              name: "Client");*/
        }
    }
}
