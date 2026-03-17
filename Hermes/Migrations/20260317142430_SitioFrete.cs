using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hermes.Migrations
{
    /// <inheritdoc />
    public partial class SitioFrete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescricaoDestino",
                table: "Fretes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescricaoOrigem",
                table: "Fretes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DistanciaExtra",
                table: "Fretes",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SitioDestino",
                table: "Fretes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SitioOrigem",
                table: "Fretes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescricaoDestino",
                table: "Fretes");

            migrationBuilder.DropColumn(
                name: "DescricaoOrigem",
                table: "Fretes");

            migrationBuilder.DropColumn(
                name: "DistanciaExtra",
                table: "Fretes");

            migrationBuilder.DropColumn(
                name: "SitioDestino",
                table: "Fretes");

            migrationBuilder.DropColumn(
                name: "SitioOrigem",
                table: "Fretes");
        }
    }
}
