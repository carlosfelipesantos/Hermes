using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hermes.Migrations
{
    /// <inheritdoc />
    public partial class Localizacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Origem",
                table: "Fretes",
                newName: "EstadoOrigem");

            migrationBuilder.RenameColumn(
                name: "Destino",
                table: "Fretes",
                newName: "EstadoDestino");

            migrationBuilder.AddColumn<int>(
                name: "TipoVeiculo",
                table: "Veiculos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Tipo",
                table: "Usuarios",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "Disponivel",
                table: "Usuarios",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Usuarios",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Usuarios",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "TotalAvaliacoes",
                table: "Usuarios",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Fretes",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "BairroDestino",
                table: "Fretes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BairroOrigem",
                table: "Fretes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CidadeDestino",
                table: "Fretes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CidadeOrigem",
                table: "Fretes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "LatitudeDestino",
                table: "Fretes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LatitudeOrigem",
                table: "Fretes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LongitudeDestino",
                table: "Fretes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "LongitudeOrigem",
                table: "Fretes",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "TipoCarga",
                table: "Fretes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TransportadorId",
                table: "Avaliacoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_TransportadorId",
                table: "Avaliacoes",
                column: "TransportadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Avaliacoes_Usuarios_TransportadorId",
                table: "Avaliacoes",
                column: "TransportadorId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avaliacoes_Usuarios_TransportadorId",
                table: "Avaliacoes");

            migrationBuilder.DropIndex(
                name: "IX_Avaliacoes_TransportadorId",
                table: "Avaliacoes");

            migrationBuilder.DropColumn(
                name: "TipoVeiculo",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "Disponivel",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "TotalAvaliacoes",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "BairroDestino",
                table: "Fretes");

            migrationBuilder.DropColumn(
                name: "BairroOrigem",
                table: "Fretes");

            migrationBuilder.DropColumn(
                name: "CidadeDestino",
                table: "Fretes");

            migrationBuilder.DropColumn(
                name: "CidadeOrigem",
                table: "Fretes");

            migrationBuilder.DropColumn(
                name: "LatitudeDestino",
                table: "Fretes");

            migrationBuilder.DropColumn(
                name: "LatitudeOrigem",
                table: "Fretes");

            migrationBuilder.DropColumn(
                name: "LongitudeDestino",
                table: "Fretes");

            migrationBuilder.DropColumn(
                name: "LongitudeOrigem",
                table: "Fretes");

            migrationBuilder.DropColumn(
                name: "TipoCarga",
                table: "Fretes");

            migrationBuilder.DropColumn(
                name: "TransportadorId",
                table: "Avaliacoes");

            migrationBuilder.RenameColumn(
                name: "EstadoOrigem",
                table: "Fretes",
                newName: "Origem");

            migrationBuilder.RenameColumn(
                name: "EstadoDestino",
                table: "Fretes",
                newName: "Destino");

            migrationBuilder.AlterColumn<string>(
                name: "Tipo",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Fretes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
