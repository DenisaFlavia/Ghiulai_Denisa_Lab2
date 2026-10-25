
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Ghiulai_Denisa_Lab2;

#nullable disable

namespace Ghiulai_Denisa_Lab2.Migrations
{
    [DbContext(typeof(Ghiulai_Denisa_Lab2Context))]
    [Migration("20261025222000_RemoveOldAuthor")]
    public partial class RemoveOldAuthor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Book");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Book",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
