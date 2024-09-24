using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalentTrail.Migrations
{
    /// <inheritdoc />
    public partial class ColumnEmployerRemove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePicturePath",
                table: "JobSeekers");

            migrationBuilder.DropColumn(
                name: "ProfilePicturePath",
                table: "Employers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePicturePath",
                table: "JobSeekers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicturePath",
                table: "Employers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
