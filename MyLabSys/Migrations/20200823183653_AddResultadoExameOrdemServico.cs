using Microsoft.EntityFrameworkCore.Migrations;

namespace MyLabSys.Migrations
{
    public partial class AddResultadoExameOrdemServico : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResultadosExamesOrdensServicos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdExameOrdemServico = table.Column<int>(nullable: false),
                    DescricaoResultado = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultadosExamesOrdensServicos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultadosExamesOrdensServicos_ExamesOrdensServicos_IdExameOrdemServico",
                        column: x => x.IdExameOrdemServico,
                        principalTable: "ExamesOrdensServicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResultadosExamesOrdensServicos_IdExameOrdemServico",
                table: "ResultadosExamesOrdensServicos",
                column: "IdExameOrdemServico",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResultadosExamesOrdensServicos");
        }
    }
}
