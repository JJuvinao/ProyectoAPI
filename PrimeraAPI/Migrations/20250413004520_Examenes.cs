using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimeraAPI.Migrations
{
    /// <inheritdoc />
    public partial class Examenes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "Examenes",
               columns: table => new
               {
                   Id = table.Column<int>(type: "int", nullable: false)
                       .Annotation("SqlServer:Identity", "1, 1"),
                   Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   Tema = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   Autor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   Tiempo = table.Column<int>(type: "int", nullable: false),
                   Privacidad = table.Column<bool>(type: "bit", nullable: false),
                   Estado = table.Column<bool>(type: "bit", nullable: false),
                   FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_Examenes", x => x.Id);
               });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Examenes");
        }
    }
}
