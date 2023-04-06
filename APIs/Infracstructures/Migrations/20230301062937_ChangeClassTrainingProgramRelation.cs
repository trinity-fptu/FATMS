using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infracstructures.Migrations
{
    /// <inheritdoc />
    public partial class ChangeClassTrainingProgramRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Class_TrainingProgramId",
                table: "Class");

            migrationBuilder.RenameColumn(
                name: "TechnicalRequirements",
                table: "Syllabus",
                newName: "TechnicalRequirements");

            migrationBuilder.CreateIndex(
                name: "IX_Class_TrainingProgramId",
                table: "Class",
                column: "TrainingProgramId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Class_TrainingProgramId",
                table: "Class");

            migrationBuilder.RenameColumn(
                name: "TechnicalRequirements",
                table: "Syllabus",
                newName: "TechnicalRequirements");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Class_TrainingProgramId",
                table: "Class",
                column: "TrainingProgramId",
                unique: true,
                filter: "[TrainingProgramId] IS NOT NULL");
        }
    }
}
