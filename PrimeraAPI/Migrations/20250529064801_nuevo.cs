using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrimeraAPI.Migrations
{
    /// <inheritdoc />
    public partial class nuevo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id_Categoria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id_Categoria);
                });

            migrationBuilder.CreateTable(
                name: "Clases",
                columns: table => new
                {
                    Id_Clase = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tema = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Autor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImagenClase = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Id_Profe = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clases", x => x.Id_Clase);
                });

            migrationBuilder.CreateTable(
                name: "Estudi_Clases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Usuario = table.Column<int>(type: "int", nullable: true),
                    Id_Clase = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estudi_Clases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Estudi_Examenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Estudiane = table.Column<int>(type: "int", nullable: true),
                    Id_Examen = table.Column<int>(type: "int", nullable: true),
                    Puntaje = table.Column<int>(type: "int", nullable: true),
                    Aciertos = table.Column<int>(type: "int", nullable: true),
                    Fallos = table.Column<int>(type: "int", nullable: true),
                    Tiempo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Nota = table.Column<float>(type: "real", nullable: true),
                    Recomendacion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estudi_Examenes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Examenes",
                columns: table => new
                {
                    Id_Examen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tema = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Autor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImagenExamen = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Id_Clase = table.Column<int>(type: "int", nullable: true),
                    Id_Juego = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Examenes", x => x.Id_Examen);
                });

            migrationBuilder.CreateTable(
                name: "Juegos",
                columns: table => new
                {
                    Id_Juego = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Genero = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tema = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Juegos", x => x.Id_Juego);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id_Usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contrasena = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Correo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Imagen = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id_Usuario);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Clases");

            migrationBuilder.DropTable(
                name: "Estudi_Clases");

            migrationBuilder.DropTable(
                name: "Estudi_Examenes");

            migrationBuilder.DropTable(
                name: "Examenes");

            migrationBuilder.DropTable(
                name: "Juegos");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
