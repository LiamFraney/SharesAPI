using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SharesAPI.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shares",
                columns: table => new
                {
                    symbol = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    currency = table.Column<string>(nullable: true),
                    lastUpdated = table.Column<DateTime>(nullable: false),
                    price = table.Column<double>(nullable: false),
                    shares = table.Column<string>(nullable: true),
                    availableShares = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shares", x => x.symbol);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    username = table.Column<string>(nullable: false),
                    password = table.Column<string>(nullable: true),
                    pennies = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.username);
                });

            migrationBuilder.CreateTable(
                name: "AquiredShares",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    quantity = table.Column<double>(nullable: false),
                    type = table.Column<string>(nullable: true),
                    username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AquiredShares", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AquiredShares_Users_username",
                        column: x => x.username,
                        principalTable: "Users",
                        principalColumn: "username",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AquiredShares_username",
                table: "AquiredShares",
                column: "username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AquiredShares");

            migrationBuilder.DropTable(
                name: "Shares");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
