using Microsoft.EntityFrameworkCore.Migrations;

namespace KidsManagement.Data.Migrations
{
    public partial class TeacherApplUserOneToOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Teachers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_ApplicationUserId",
                table: "Teachers",
                column: "ApplicationUserId",
                unique: true,
                filter: "[ApplicationUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_AspNetUsers_ApplicationUserId",
                table: "Teachers",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_AspNetUsers_ApplicationUserId",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Teachers_ApplicationUserId",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Teachers");
        }
    }
}
