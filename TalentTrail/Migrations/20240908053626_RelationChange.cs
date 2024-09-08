using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalentTrail.Migrations
{
    /// <inheritdoc />
    public partial class RelationChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Employers_EmployerId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_EmployerId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmployerId",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployerId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmployerId",
                table: "Users",
                column: "EmployerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Employers_EmployerId",
                table: "Users",
                column: "EmployerId",
                principalTable: "Employers",
                principalColumn: "EmployerId");
        }
    }
}
