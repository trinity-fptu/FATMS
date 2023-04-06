using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infracstructures.Migrations
{
    /// <inheritdoc />
    public partial class updateSyllabus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TrainingPrograms_CreatedBy",
                table: "TrainingPrograms");

            migrationBuilder.DropIndex(
                name: "IX_TrainingPrograms_LastModifyBy",
                table: "TrainingPrograms");

            migrationBuilder.DropIndex(
                name: "IX_TMS_CheckedBy",
                table: "TMS");

            migrationBuilder.DropIndex(
                name: "IX_Syllabus_CreatedBy",
                table: "Syllabus");

            migrationBuilder.DropIndex(
                name: "IX_Syllabus_LastModifiedBy",
                table: "Syllabus");

            migrationBuilder.DropIndex(
                name: "IX_Class_ApprovedBy",
                table: "Class");

            migrationBuilder.DropIndex(
                name: "IX_Class_CreatedBy",
                table: "Class");

            migrationBuilder.AddColumn<string>(
                name: "TechicalRequirements",
                table: "Syllabus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPrograms_CreatedBy",
                table: "TrainingPrograms",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPrograms_LastModifyBy",
                table: "TrainingPrograms",
                column: "LastModifyBy");

            migrationBuilder.CreateIndex(
                name: "IX_TMS_CheckedBy",
                table: "TMS",
                column: "CheckedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Syllabus_CreatedBy",
                table: "Syllabus",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Syllabus_LastModifiedBy",
                table: "Syllabus",
                column: "LastModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Class_ApprovedBy",
                table: "Class",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Class_CreatedBy",
                table: "Class",
                column: "CreatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TrainingPrograms_CreatedBy",
                table: "TrainingPrograms");

            migrationBuilder.DropIndex(
                name: "IX_TrainingPrograms_LastModifyBy",
                table: "TrainingPrograms");

            migrationBuilder.DropIndex(
                name: "IX_TMS_CheckedBy",
                table: "TMS");

            migrationBuilder.DropIndex(
                name: "IX_Syllabus_CreatedBy",
                table: "Syllabus");

            migrationBuilder.DropIndex(
                name: "IX_Syllabus_LastModifiedBy",
                table: "Syllabus");

            migrationBuilder.DropIndex(
                name: "IX_Class_ApprovedBy",
                table: "Class");

            migrationBuilder.DropIndex(
                name: "IX_Class_CreatedBy",
                table: "Class");

            migrationBuilder.DropColumn(
                name: "TechicalRequirements",
                table: "Syllabus");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPrograms_CreatedBy",
                table: "TrainingPrograms",
                column: "CreatedBy",
                unique: true,
                filter: "[CreatedBy] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPrograms_LastModifyBy",
                table: "TrainingPrograms",
                column: "LastModifyBy",
                unique: true,
                filter: "[LastModifyBy] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TMS_CheckedBy",
                table: "TMS",
                column: "CheckedBy",
                unique: true,
                filter: "[CheckedBy] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Syllabus_CreatedBy",
                table: "Syllabus",
                column: "CreatedBy",
                unique: true,
                filter: "[CreatedBy] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Syllabus_LastModifiedBy",
                table: "Syllabus",
                column: "LastModifiedBy",
                unique: true,
                filter: "[LastModifiedBy] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Class_ApprovedBy",
                table: "Class",
                column: "ApprovedBy",
                unique: true,
                filter: "[ApprovedBy] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Class_CreatedBy",
                table: "Class",
                column: "CreatedBy",
                unique: true,
                filter: "[CreatedBy] IS NOT NULL");
        }
    }
}