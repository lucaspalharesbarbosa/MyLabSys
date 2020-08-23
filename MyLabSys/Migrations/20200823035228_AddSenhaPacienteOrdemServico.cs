using Microsoft.EntityFrameworkCore.Migrations;

namespace MyLabSys.Migrations
{
    public partial class AddSenhaPacienteOrdemServico : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SenhaPaciente",
                table: "OrdensServicos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenhaPaciente",
                table: "OrdensServicos");
        }
    }
}
