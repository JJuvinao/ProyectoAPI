using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimeraAPI.Migrations
{
    /// <inheritdoc />
    public partial class cambios2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tiempo",
                table: "Estudi_Examenes");

            migrationBuilder.RenameColumn(
                name: "Puntaje",
                table: "Estudi_Examenes",
                newName: "Intentos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Intentos",
                table: "Estudi_Examenes",
                newName: "Puntaje");

            migrationBuilder.AddColumn<DateTime>(
                name: "Tiempo",
                table: "Estudi_Examenes",
                type: "datetime2",
                nullable: true);
        }
    }
}
