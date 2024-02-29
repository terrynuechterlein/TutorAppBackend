using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorAppBackend.Migrations
{
    /// <inheritdoc />
    public partial class UserModel_Major : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Major",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Major",
                table: "AspNetUsers");
        }
    }
}
