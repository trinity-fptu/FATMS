using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infracstructures.Migrations
{
    /// <inheritdoc />
    public partial class OutputStandardLectureRelationFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lectures_OutputStandardId",
                table: "Lectures");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_OutputStandardId",
                table: "Lectures",
                column: "OutputStandardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Lectures_OutputStandardId",
                table: "Lectures");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_OutputStandardId",
                table: "Lectures",
                column: "OutputStandardId",
                unique: true,
                filter: "[OutputStandardId] IS NOT NULL");
        }
    }
}
