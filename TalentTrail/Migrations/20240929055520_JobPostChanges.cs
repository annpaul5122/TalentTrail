using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalentTrail.Migrations
{
    /// <inheritdoc />
    public partial class JobPostChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateIndex(
                name: "IX_JobPosts_CompanyId",
                table: "JobPosts",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPosts_CompanyDetails_CompanyId",
                table: "JobPosts",
                column: "CompanyId",
                principalTable: "CompanyDetails",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobPosts_CompanyDetails_CompanyId",
                table: "JobPosts");

            migrationBuilder.DropIndex(
                name: "IX_JobPosts_CompanyId",
                table: "JobPosts");

            migrationBuilder.AddColumn<int>(
                name: "CompanyDetailsCompanyId",
                table: "JobPosts",
                type: "int",
                nullable: true);

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
    }
}
