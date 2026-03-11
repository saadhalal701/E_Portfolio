using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEB_SH.Migrations
{
    /// <inheritdoc />
    public partial class Entreprise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Competence_Personnes",
                table: "Competence");

            migrationBuilder.DropColumn(
                name: "Titre",
                table: "Competence");

            migrationBuilder.AlterColumn<string>(
                name: "Titre",
                table: "Projet",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "PersonneId",
                table: "Projet",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nom",
                table: "Competence",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Entreprises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreePar = table.Column<int>(type: "int", nullable: false),
                    EstSupprime = table.Column<bool>(type: "bit", nullable: true),
                    SupprimeA = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SupprimePar = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entreprises", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Competence_Personnes_PersonneId",
                table: "Competence",
                column: "PersonneId",
                principalTable: "Personnes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Competence_Personnes_PersonneId",
                table: "Competence");

            migrationBuilder.DropTable(
                name: "Entreprises");

            migrationBuilder.AlterColumn<string>(
                name: "Titre",
                table: "Projet",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<int>(
                name: "PersonneId",
                table: "Projet",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Nom",
                table: "Competence",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Titre",
                table: "Competence",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Competence_Personnes_PersonneId",
                table: "Competence",
                column: "PersonneId",
                principalTable: "Personnes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
