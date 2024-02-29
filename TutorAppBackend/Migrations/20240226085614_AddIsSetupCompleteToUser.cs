using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorAppBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddIsSetupCompleteToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSetupComplete",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSetupComplete",
                table: "AspNetUsers");
        }
    }
}
