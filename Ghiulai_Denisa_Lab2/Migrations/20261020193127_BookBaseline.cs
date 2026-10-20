
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ghiulai_Denisa_Lab2.Migrations
{
    public partial class BookBaseline : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Tabela Book exista deja.
            // Nu modificam baza de date.
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Nu stergem tabela Book.
        }
    }
}
