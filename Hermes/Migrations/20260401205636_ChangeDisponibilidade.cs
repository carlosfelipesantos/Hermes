using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hermes.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDisponibilidade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Disponibilidades");

            migrationBuilder.DropColumn(
                name: "HoraAgendada",
                table: "Fretes");

            migrationBuilder.RenameColumn(
                name: "DataAgendada",
                table: "Fretes",
                newName: "DataHoraFimReal");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataHoraFimPrevisto",
                table: "Fretes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DataHoraInicio",
                table: "Fretes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "DuracaoEstimada",
                table: "Fretes",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateTable(
                name: "DisponibilidadesBase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransportadorId = table.Column<int>(type: "int", nullable: false),
                    DiaSemana = table.Column<int>(type: "int", nullable: false),
                    HoraInicio = table.Column<TimeSpan>(type: "time", nullable: false),
                    HoraFim = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisponibilidadesBase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DisponibilidadesBase_Usuarios_TransportadorId",
                        column: x => x.TransportadorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DisponibilidadesBase_TransportadorId",
                table: "DisponibilidadesBase",
                column: "TransportadorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisponibilidadesBase");

            migrationBuilder.DropColumn(
                name: "DataHoraFimPrevisto",
                table: "Fretes");

            migrationBuilder.DropColumn(
                name: "DataHoraInicio",
                table: "Fretes");

            migrationBuilder.DropColumn(
                name: "DuracaoEstimada",
                table: "Fretes");

            migrationBuilder.RenameColumn(
                name: "DataHoraFimReal",
                table: "Fretes",
                newName: "DataAgendada");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HoraAgendada",
                table: "Fretes",
                type: "time",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Disponibilidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransportadorId = table.Column<int>(type: "int", nullable: false),
                    DiaSemana = table.Column<int>(type: "int", nullable: false),
                    Hora = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disponibilidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Disponibilidades_Usuarios_TransportadorId",
                        column: x => x.TransportadorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Disponibilidades_TransportadorId",
                table: "Disponibilidades",
                column: "TransportadorId");
        }
    }
}
