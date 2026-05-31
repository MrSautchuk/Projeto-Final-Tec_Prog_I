using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoGaragem.Migrations
{
    /// <inheritdoc />
    public partial class AddFotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FotoBase64",
                table: "Veiculos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FotoBase64",
                table: "Usuarios",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FotoBase64",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "FotoBase64",
                table: "Usuarios");
        }
    }
}
