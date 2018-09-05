using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gangstabit.DataAccess.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Gangstabit");

            migrationBuilder.CreateTable(
                name: "Game",
                schema: "Gangstabit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EndDate = table.Column<DateTime>(nullable: false),
                    Multiplier = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                schema: "Gangstabit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 25, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bet",
                schema: "Gangstabit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PlayerId = table.Column<int>(nullable: false),
                    GameId = table.Column<int>(nullable: false),
                    Target = table.Column<double>(nullable: false),
                    Wage = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bet_Game_GameId",
                        column: x => x.GameId,
                        principalSchema: "Gangstabit",
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bet_Player_PlayerId",
                        column: x => x.PlayerId,
                        principalSchema: "Gangstabit",
                        principalTable: "Player",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bet_GameId",
                schema: "Gangstabit",
                table: "Bet",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Bet_PlayerId",
                schema: "Gangstabit",
                table: "Bet",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bet",
                schema: "Gangstabit");

            migrationBuilder.DropTable(
                name: "Game",
                schema: "Gangstabit");

            migrationBuilder.DropTable(
                name: "Player",
                schema: "Gangstabit");
        }
    }
}
