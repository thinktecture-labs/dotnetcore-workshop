using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GrpcService.Migrations
{
    public partial class AddsVorgang2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vorgaenge_Akten_AktenNummer",
                table: "Vorgaenge");

            migrationBuilder.DropIndex(
                name: "IX_Vorgaenge_AktenNummer",
                table: "Vorgaenge");

            migrationBuilder.AlterColumn<Guid>(
                name: "AktenNummer",
                table: "Vorgaenge",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AktenNummer1",
                table: "Vorgaenge",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vorgaenge_AktenNummer1",
                table: "Vorgaenge",
                column: "AktenNummer1");

            migrationBuilder.AddForeignKey(
                name: "FK_Vorgaenge_Akten_AktenNummer1",
                table: "Vorgaenge",
                column: "AktenNummer1",
                principalTable: "Akten",
                principalColumn: "AktenNummer",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vorgaenge_Akten_AktenNummer1",
                table: "Vorgaenge");

            migrationBuilder.DropIndex(
                name: "IX_Vorgaenge_AktenNummer1",
                table: "Vorgaenge");

            migrationBuilder.DropColumn(
                name: "AktenNummer1",
                table: "Vorgaenge");

            migrationBuilder.AlterColumn<Guid>(
                name: "AktenNummer",
                table: "Vorgaenge",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.CreateIndex(
                name: "IX_Vorgaenge_AktenNummer",
                table: "Vorgaenge",
                column: "AktenNummer");

            migrationBuilder.AddForeignKey(
                name: "FK_Vorgaenge_Akten_AktenNummer",
                table: "Vorgaenge",
                column: "AktenNummer",
                principalTable: "Akten",
                principalColumn: "AktenNummer",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
