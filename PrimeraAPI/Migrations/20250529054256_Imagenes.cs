using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimeraAPI.Migrations
{
    /// <inheritdoc />
    public partial class Imagenes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Imagen",
                table: "Usuarios",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImagenExamen",
                table: "Examenes",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImagenClase",
                table: "Clases",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagen",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "ImagenExamen",
                table: "Examenes");

            migrationBuilder.DropColumn(
                name: "ImagenClase",
                table: "Clases");
        }
    }
}
