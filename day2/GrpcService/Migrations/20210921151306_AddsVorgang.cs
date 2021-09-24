using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrpcService.Migrations
{
    public partial class AddsVorgang : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vorgaenge",
                columns: table => new
                {
                    VorangsNummer = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    AktenNummer = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vorgaenge", x => x.VorangsNummer);
                    table.ForeignKey(
                        name: "FK_Vorgaenge_Akten_AktenNummer",
                        column: x => x.AktenNummer,
                        principalTable: "Akten",
                        principalColumn: "AktenNummer",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vorgaenge_AktenNummer",
                table: "Vorgaenge",
                column: "AktenNummer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vorgaenge");
        }
    }
}
