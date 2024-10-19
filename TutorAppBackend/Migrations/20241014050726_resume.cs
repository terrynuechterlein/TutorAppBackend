using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorAppBackend.Migrations
{
    /// <inheritdoc />
    public partial class resume : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResumeUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResumeUrl",
                table: "AspNetUsers");
        }
    }
}
