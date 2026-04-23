using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hermes.Migrations
{
    /// <inheritdoc />
    public partial class ApiFinalizadaTalvez : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Veiculos_TransportadorId",
                table: "Veiculos");

            migrationBuilder.DropIndex(
                name: "IX_Notificacoes_UsuarioId",
                table: "Notificacoes");

            migrationBuilder.DropIndex(
                name: "IX_Fretes_TransportadorId",
                table: "Fretes");

            migrationBuilder.DropIndex(
                name: "IX_DisponibilidadesBase_TransportadorId",
                table: "DisponibilidadesBase");

            migrationBuilder.AlterColumn<double>(
                name: "LongitudeOrigem",
                table: "Fretes",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "LongitudeDestino",
                table: "Fretes",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "LatitudeOrigem",
                table: "Fretes",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "LatitudeDestino",
                table: "Fretes",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "CidadeOrigem",
                table: "Fretes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_TransportadorId_TipoVeiculo",
                table: "Veiculos",
                columns: new[] { "TransportadorId", "TipoVeiculo" });

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_UsuarioId_DataCriacao",
                table: "Notificacoes",
                columns: new[] { "UsuarioId", "DataCriacao" });

            migrationBuilder.CreateIndex(
                name: "IX_Fretes_CidadeOrigem",
                table: "Fretes",
                column: "CidadeOrigem");

            migrationBuilder.CreateIndex(
                name: "IX_Fretes_Status_DataSolicitacao",
                table: "Fretes",
                columns: new[] { "Status", "DataSolicitacao" });

            migrationBuilder.CreateIndex(
                name: "IX_Fretes_TransportadorId_Status",
                table: "Fretes",
                columns: new[] { "TransportadorId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_DisponibilidadesBase_TransportadorId_DiaSemana",
                table: "DisponibilidadesBase",
                columns: new[] { "TransportadorId", "DiaSemana" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Veiculos_TransportadorId_TipoVeiculo",
                table: "Veiculos");

            migrationBuilder.DropIndex(
                name: "IX_Notificacoes_UsuarioId_DataCriacao",
                table: "Notificacoes");

            migrationBuilder.DropIndex(
                name: "IX_Fretes_CidadeOrigem",
                table: "Fretes");

            migrationBuilder.DropIndex(
                name: "IX_Fretes_Status_DataSolicitacao",
                table: "Fretes");

            migrationBuilder.DropIndex(
                name: "IX_Fretes_TransportadorId_Status",
                table: "Fretes");

            migrationBuilder.DropIndex(
                name: "IX_DisponibilidadesBase_TransportadorId_DiaSemana",
                table: "DisponibilidadesBase");

            migrationBuilder.AlterColumn<double>(
                name: "LongitudeOrigem",
                table: "Fretes",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LongitudeDestino",
                table: "Fretes",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LatitudeOrigem",
                table: "Fretes",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "LatitudeDestino",
                table: "Fretes",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CidadeOrigem",
                table: "Fretes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_TransportadorId",
                table: "Veiculos",
                column: "TransportadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_UsuarioId",
                table: "Notificacoes",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Fretes_TransportadorId",
                table: "Fretes",
                column: "TransportadorId");

            migrationBuilder.CreateIndex(
                name: "IX_DisponibilidadesBase_TransportadorId",
                table: "DisponibilidadesBase",
                column: "TransportadorId");
        }
    }
}
