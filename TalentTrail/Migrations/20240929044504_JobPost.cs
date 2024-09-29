using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalentTrail.Migrations
{
    /// <inheritdoc />
    public partial class JobPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyDetailsCompanyId",
                table: "JobPosts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "JobPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_JobPosts_CompanyDetailsCompanyId",
                table: "JobPosts",
                column: "CompanyDetailsCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPosts_CompanyDetails_CompanyDetailsCompanyId",
                table: "JobPosts",
                column: "CompanyDetailsCompanyId",
                principalTable: "CompanyDetails",
                principalColumn: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobPosts_CompanyDetails_CompanyDetailsCompanyId",
                table: "JobPosts");

            migrationBuilder.DropIndex(
                name: "IX_JobPosts_CompanyDetailsCompanyId",
                table: "JobPosts");

            migrationBuilder.DropColumn(
                name: "CompanyDetailsCompanyId",
                table: "JobPosts");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "JobPosts");
        }
    }
}
