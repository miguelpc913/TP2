using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistencia.Migrations
{
    public partial class Final12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Categoria_Tipo",
                table: "Categoria");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Categoria_Tipo",
                table: "Categoria",
                column: "Tipo",
                unique: true,
                filter: "[Tipo] IS NOT NULL");
        }
    }
}
