using Microsoft.EntityFrameworkCore.Migrations;

namespace KidsManagement.Data.Migrations
{
    public partial class profilePicUri : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePicURI",
                table: "Teachers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicURI",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicURI",
                table: "Admins",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePicURI",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "ProfilePicURI",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ProfilePicURI",
                table: "Admins");
        }
    }
}
