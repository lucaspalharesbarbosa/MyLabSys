using Microsoft.EntityFrameworkCore.Migrations;

namespace MyLabSys.Migrations
{
    public partial class AddTabelaConveniosEColunaStatusNaOrdemServico : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NomeConvenio",
                table: "OrdensServicos");

            migrationBuilder.AddColumn<int>(
                name: "IdConvenio",
                table: "Pacientes",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Status",
                table: "OrdensServicos",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateTable(
                name: "Convenios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(maxLength: 100, nullable: true),
                    PercentualDesconto = table.Column<decimal>(type: "decimal(18, 2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Convenios", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_IdConvenio",
                table: "Pacientes",
                column: "IdConvenio");

            migrationBuilder.AddForeignKey(
                name: "FK_Pacientes_Convenios_IdConvenio",
                table: "Pacientes",
                column: "IdConvenio",
                principalTable: "Convenios",
                principalColumn: "Id");

            migrationBuilder.Sql("INSERT INTO Convenios(Nome, PercentualDesconto) VALUES('Amil', 25.00)");
            migrationBuilder.Sql("INSERT INTO Convenios(Nome, PercentualDesconto) VALUES('Unimed', 20.00)");
            migrationBuilder.Sql("INSERT INTO Convenios(Nome, PercentualDesconto) VALUES('Austa', 18.00)");
            migrationBuilder.Sql("INSERT INTO Convenios(Nome, PercentualDesconto) VALUES('Bensaúde', 15.00)");
            migrationBuilder.Sql("INSERT INTO Convenios(Nome, PercentualDesconto) VALUES('Intermédica', 13.00)");
            migrationBuilder.Sql("INSERT INTO Convenios(Nome, PercentualDesconto) VALUES('Santa Casa Saúde', 10.00)");

            migrationBuilder.Sql("UPDATE Pacientes SET IdConvenio = 3 WHERE Id = 1");
            migrationBuilder.Sql("UPDATE Pacientes SET IdConvenio = 1 WHERE Id = 2");
            migrationBuilder.Sql("UPDATE Pacientes SET IdConvenio = 2 WHERE Id = 3");
            migrationBuilder.Sql("UPDATE Pacientes SET IdConvenio = 3 WHERE Id = 4");
            migrationBuilder.Sql("UPDATE Pacientes SET IdConvenio = 4 WHERE Id = 5");
            migrationBuilder.Sql("UPDATE Pacientes SET IdConvenio = 5 WHERE Id = 6");
            migrationBuilder.Sql("UPDATE Pacientes SET IdConvenio = 2 WHERE Id = 7");
            migrationBuilder.Sql("UPDATE Pacientes SET IdConvenio = 6 WHERE Id = 8");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pacientes_Convenios_IdConvenio",
                table: "Pacientes");

            migrationBuilder.DropTable(
                name: "Convenios");

            migrationBuilder.DropIndex(
                name: "IX_Pacientes_IdConvenio",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "IdConvenio",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "OrdensServicos");

            migrationBuilder.AddColumn<string>(
                name: "NomeConvenio",
                table: "OrdensServicos",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);
        }
    }
}
