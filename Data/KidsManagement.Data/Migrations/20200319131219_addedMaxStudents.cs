using Microsoft.EntityFrameworkCore.Migrations;

namespace KidsManagement.Data.Migrations
{
    public partial class addedMaxStudents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxStudents",
                table: "Groups",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxStudents",
                table: "Groups");
        }
    }
}
