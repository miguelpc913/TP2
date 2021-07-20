using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistencia.Migrations
{
    public partial class Final2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gastos_UserId",
                table: "Gastos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_UserId",
                table: "Categoria",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categoria_Users_UserId",
                table: "Categoria",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gastos_Users_UserId",
                table: "Gastos",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categoria_Users_UserId",
                table: "Categoria");

            migrationBuilder.DropForeignKey(
                name: "FK_Gastos_Users_UserId",
                table: "Gastos");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Gastos_UserId",
                table: "Gastos");

            migrationBuilder.DropIndex(
                name: "IX_Categoria_UserId",
                table: "Categoria");
        }
    }
}
