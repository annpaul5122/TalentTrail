using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalentTrail.Migrations
{
    /// <inheritdoc />
    public partial class JobPostChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Industry",
                table: "JobPosts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Industry",
                table: "JobPosts");
        }
    }
}
