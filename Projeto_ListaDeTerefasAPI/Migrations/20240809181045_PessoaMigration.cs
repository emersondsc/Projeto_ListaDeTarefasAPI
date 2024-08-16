using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projeto_ListaDeTerefasAPI.Migrations
{
    /// <inheritdoc />
    public partial class PessoaMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pessoa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pessoa", x => x.Id);
                });

            // Add a foreign key to the Tarefa table referencing the Pessoa table
            migrationBuilder.AddColumn<int>(
                name: "PessoaId",
                table: "Tarefa",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tarefa_PessoaId",
                table: "Tarefa",
                column: "PessoaId");

            // Add a foreign key constraint referencing the Pessoa table
            migrationBuilder.AddForeignKey(
                name: "FK_Tarefa_Pessoa_PessoaId",
                table: "Tarefa",
                column: "PessoaId",
                principalTable: "Pessoa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tarefa_Pessoa_PessoaId",
                table: "Tarefa");

            migrationBuilder.DropIndex(
                name: "IX_Tarefa_PessoaId",
                table: "Tarefa");

            migrationBuilder.DropColumn(
                name: "PessoaId",
                table: "Tarefa");

            migrationBuilder.DropTable(
                name: "Pessoa");
        }
    }

}
