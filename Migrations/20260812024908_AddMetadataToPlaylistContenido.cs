using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpotifyClone.API.Migrations
{
    /// <inheritdoc />
    public partial class AddMetadataToPlaylistContenido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Creador",
                table: "PlaylistContenidos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "DuracionMs",
                table: "PlaylistContenidos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImagenUrl",
                table: "PlaylistContenidos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "PlaylistContenidos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creador",
                table: "PlaylistContenidos");

            migrationBuilder.DropColumn(
                name: "DuracionMs",
                table: "PlaylistContenidos");

            migrationBuilder.DropColumn(
                name: "ImagenUrl",
                table: "PlaylistContenidos");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "PlaylistContenidos");
        }
    }
}
