using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalentTrail.Migrations
{
    /// <inheritdoc />
    public partial class JobseekerChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_JobSeekers_JobSeekerSeekerId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_JobSeekerSeekerId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "JobSeekerSeekerId",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobSeekerSeekerId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_JobSeekerSeekerId",
                table: "Users",
                column: "JobSeekerSeekerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_JobSeekers_JobSeekerSeekerId",
                table: "Users",
                column: "JobSeekerSeekerId",
                principalTable: "JobSeekers",
                principalColumn: "SeekerId");
        }
    }
}
