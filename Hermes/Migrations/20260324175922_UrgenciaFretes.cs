using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hermes.Migrations
{
    /// <inheritdoc />
    public partial class UrgenciaFretes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Urgente",
                table: "Fretes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Urgente",
                table: "Fretes");
        }
    }
}
