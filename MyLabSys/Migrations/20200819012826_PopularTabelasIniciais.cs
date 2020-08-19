using Microsoft.EntityFrameworkCore.Migrations;

namespace MyLabSys.Migrations
{
    public partial class PopularTabelasIniciais : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Medicos(Nome, Especialidade) VALUES('Alberto José','Pediatra')");
            migrationBuilder.Sql("INSERT INTO Medicos(Nome, Especialidade) VALUES('Beatriz Lima','Cardiologista')");
            migrationBuilder.Sql("INSERT INTO Medicos(Nome, Especialidade) VALUES('Carlos da Silva','Dermatologista')");
            migrationBuilder.Sql("INSERT INTO Medicos(Nome, Especialidade) VALUES('Fábio Rodrigues','Infectologista')");
            migrationBuilder.Sql("INSERT INTO Medicos(Nome, Especialidade) VALUES('Gabriel Simões','Neurologista')");
            migrationBuilder.Sql("INSERT INTO Medicos(Nome, Especialidade) VALUES('José Cristovão','Psiquiatria')");
            migrationBuilder.Sql("INSERT INTO Medicos(Nome, Especialidade) VALUES('Lucas Medeiros','Urologista')");

            migrationBuilder.Sql("INSERT INTO Pacientes(Nome, DataNascimento, Sexo, Endereco) VALUES('Bruna Carmona', '1995-08-10 00:00:00.000', 2, 'Rua Coronel Bento, 123, Ipiranga')");
            migrationBuilder.Sql("INSERT INTO Pacientes(Nome, DataNascimento, Sexo, Endereco) VALUES('Cláudio das Neves', '1993-05-19 00:00:00.000', 1, 'Rua Mato Seco, 236, Vila Selmo')");
            migrationBuilder.Sql("INSERT INTO Pacientes(Nome, DataNascimento, Sexo, Endereco) VALUES('Deberto de Souza', '1980-03-20 00:00:00.000', 1, 'Rua das Pedras Negras, 598, Boa Vista')");
            migrationBuilder.Sql("INSERT INTO Pacientes(Nome, DataNascimento, Sexo, Endereco) VALUES('Isabela Fernandes', '1990-11-30 00:00:00.000', 2, 'Rua José Freitas, 9541, Jaguaré')");
            migrationBuilder.Sql("INSERT INTO Pacientes(Nome, DataNascimento, Sexo, Endereco) VALUES('Heitor Mendes', '1998-02-01 00:00:00.000', 1, 'Avenida Brasilandia, 541, Vila São Matheus')");
            migrationBuilder.Sql("INSERT INTO Pacientes(Nome, DataNascimento, Sexo, Endereco) VALUES('Otávio Lucas', '1989-12-09 00:00:00.000', 1, 'Rua Santo Expedito, 1225, Bosque Feliz')");
            migrationBuilder.Sql("INSERT INTO Pacientes(Nome, DataNascimento, Sexo, Endereco) VALUES('Wilian Neto', '2000-06-22 00:00:00.000', 1, 'Rua Silva Santos, 1123, Nova Esperança')");

            migrationBuilder.Sql("INSERT INTO Exames(Descricao, Preco) VALUES('Hemograma', 45.00)");
            migrationBuilder.Sql("INSERT INTO Exames(Descricao, Preco) VALUES('Glicemia', 38.00)");
            migrationBuilder.Sql("INSERT INTO Exames(Descricao, Preco) VALUES('Exame de urina', 75.00)");
            migrationBuilder.Sql("INSERT INTO Exames(Descricao, Preco) VALUES('Exame de fezes', 62.00)");
            migrationBuilder.Sql("INSERT INTO Exames(Descricao, Preco) VALUES('VLDR', 45.00)");
            migrationBuilder.Sql("INSERT INTO Exames(Descricao, Preco) VALUES('Colesterol', 54.00)");
            migrationBuilder.Sql("INSERT INTO Exames(Descricao, Preco) VALUES('TGO (AST) TGP (ALP)', 115.00)");

            migrationBuilder.Sql("INSERT INTO PostosColetas(Descricao, Endereco) VALUES('Laboratório Matriz', 'Rua José Castro, 254, Redentora')");
            migrationBuilder.Sql("INSERT INTO PostosColetas(Descricao, Endereco) VALUES('Laboratório Filial 1', 'Rua São Benedito, 112, Auróra')");
            migrationBuilder.Sql("INSERT INTO PostosColetas(Descricao, Endereco) VALUES('Laboratório Filial 2', 'Rua Capitão do Lago, 547, Jardim do Lago')");
            migrationBuilder.Sql("INSERT INTO PostosColetas(Descricao, Endereco) VALUES('Laboratório Filial 3', 'Avenida 25 de Janeiro, 123, Bela Rua')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Medicos");
            migrationBuilder.Sql("DELETE FROM Pacientes");
            migrationBuilder.Sql("DELETE FROM Exames");
            migrationBuilder.Sql("DELETE FROM PostosColetas");
        }
    }
}
