using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyLabSys.Migrations
{
    public partial class CriarVinculosOrdemServicoEExameOrdemServico : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrdensServicos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodigoPedidoMedico = table.Column<string>(maxLength: 100, nullable: true),
                    CodigoProtocolo = table.Column<string>(maxLength: 100, nullable: true),
                    DataEmissao = table.Column<DateTime>(nullable: false),
                    DataPrevisaoEntrega = table.Column<DateTime>(nullable: false),
                    IdPaciente = table.Column<int>(nullable: false),
                    NomeConvenio = table.Column<string>(maxLength: 150, nullable: true),
                    IdPostoColeta = table.Column<int>(nullable: false),
                    IdMedico = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdensServicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdensServicos_Medicos_IdMedico",
                        column: x => x.IdMedico,
                        principalTable: "Medicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdensServicos_Pacientes_IdPaciente",
                        column: x => x.IdPaciente,
                        principalTable: "Pacientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdensServicos_PostosColetas_IdPostoColeta",
                        column: x => x.IdPostoColeta,
                        principalTable: "PostosColetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExamesOrdensServicos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdOrdemServico = table.Column<int>(nullable: false),
                    IdExame = table.Column<int>(nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamesOrdensServicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamesOrdensServicos_Exames_IdExame",
                        column: x => x.IdExame,
                        principalTable: "Exames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExamesOrdensServicos_OrdensServicos_IdOrdemServico",
                        column: x => x.IdOrdemServico,
                        principalTable: "OrdensServicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamesOrdensServicos_IdExame",
                table: "ExamesOrdensServicos",
                column: "IdExame");

            migrationBuilder.CreateIndex(
                name: "IX_ExamesOrdensServicos_IdOrdemServico",
                table: "ExamesOrdensServicos",
                column: "IdOrdemServico");

            migrationBuilder.CreateIndex(
                name: "IX_OrdensServicos_IdMedico",
                table: "OrdensServicos",
                column: "IdMedico");

            migrationBuilder.CreateIndex(
                name: "IX_OrdensServicos_IdPaciente",
                table: "OrdensServicos",
                column: "IdPaciente");

            migrationBuilder.CreateIndex(
                name: "IX_OrdensServicos_IdPostoColeta",
                table: "OrdensServicos",
                column: "IdPostoColeta");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamesOrdensServicos");

            migrationBuilder.DropTable(
                name: "OrdensServicos");
        }
    }
}
