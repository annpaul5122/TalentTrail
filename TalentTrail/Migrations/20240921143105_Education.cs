using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalentTrail.Migrations
{
    /// <inheritdoc />
    public partial class Education : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Certifications",
                table: "JobSeekers");

            migrationBuilder.DropColumn(
                name: "Education",
                table: "JobSeekers");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicturePath",
                table: "JobSeekers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Certifications",
                columns: table => new
                {
                    CertificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CertificationName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CertificateImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DateIssued = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SeekerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certifications", x => x.CertificationId);
                    table.ForeignKey(
                        name: "FK_Certifications_JobSeekers_SeekerId",
                        column: x => x.SeekerId,
                        principalTable: "JobSeekers",
                        principalColumn: "SeekerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Educations",
                columns: table => new
                {
                    EducationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Degree = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Institution = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PassoutYear = table.Column<int>(type: "int", nullable: false),
                    SeekerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educations", x => x.EducationId);
                    table.ForeignKey(
                        name: "FK_Educations_JobSeekers_SeekerId",
                        column: x => x.SeekerId,
                        principalTable: "JobSeekers",
                        principalColumn: "SeekerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Certifications_SeekerId",
                table: "Certifications",
                column: "SeekerId");

            migrationBuilder.CreateIndex(
                name: "IX_Educations_SeekerId",
                table: "Educations",
                column: "SeekerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Certifications");

            migrationBuilder.DropTable(
                name: "Educations");

            migrationBuilder.DropColumn(
                name: "ProfilePicturePath",
                table: "JobSeekers");

            migrationBuilder.AddColumn<string>(
                name: "Certifications",
                table: "JobSeekers",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Education",
                table: "JobSeekers",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
