using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infracstructures.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Syllabus_TrainingDeliveryPrinciple_TrainingDeliveryPrincipleId",
                table: "Syllabus");

            migrationBuilder.DropTable(
                name: "TrainingDeliveryPrinciple");

            migrationBuilder.DropIndex(
                name: "IX_Syllabus_TrainingDeliveryPrincipleId",
                table: "Syllabus");

            migrationBuilder.DropColumn(
                name: "TrainingDeliveryPrincipleId",
                table: "Syllabus");

            migrationBuilder.DropColumn(
                name: "MorningStatus",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "NightStatus",
                table: "Attendances");

            migrationBuilder.RenameColumn(
                name: "NoonStatus",
                table: "Attendances",
                newName: "AttendanceStatus");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Units",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DaysDuration",
                table: "TrainingPrograms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeDuration",
                table: "TrainingPrograms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ApproveStatus",
                table: "TMS",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DaysDuration",
                table: "Syllabus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeDuration",
                table: "Syllabus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TrainingDeliveryPrinciple",
                table: "Syllabus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DaysDuration",
                table: "Class",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeDuration",
                table: "Class",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ClassSyllabus",
                columns: table => new
                {
                    ClassesId = table.Column<int>(type: "int", nullable: false),
                    SyllabusesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSyllabus", x => new { x.ClassesId, x.SyllabusesId });
                    table.ForeignKey(
                        name: "FK_ClassSyllabus_Class_ClassesId",
                        column: x => x.ClassesId,
                        principalTable: "Class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassSyllabus_Syllabus_SyllabusesId",
                        column: x => x.SyllabusesId,
                        principalTable: "Syllabus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassSyllabus_SyllabusesId",
                table: "ClassSyllabus",
                column: "SyllabusesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassSyllabus");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Units");

            migrationBuilder.DropColumn(
                name: "DaysDuration",
                table: "TrainingPrograms");

            migrationBuilder.DropColumn(
                name: "TimeDuration",
                table: "TrainingPrograms");

            migrationBuilder.DropColumn(
                name: "ApproveStatus",
                table: "TMS");

            migrationBuilder.DropColumn(
                name: "DaysDuration",
                table: "Syllabus");

            migrationBuilder.DropColumn(
                name: "TimeDuration",
                table: "Syllabus");

            migrationBuilder.DropColumn(
                name: "TrainingDeliveryPrinciple",
                table: "Syllabus");

            migrationBuilder.DropColumn(
                name: "DaysDuration",
                table: "Class");

            migrationBuilder.DropColumn(
                name: "TimeDuration",
                table: "Class");

            migrationBuilder.RenameColumn(
                name: "AttendanceStatus",
                table: "Attendances",
                newName: "NoonStatus");

            migrationBuilder.AddColumn<int>(
                name: "TrainingDeliveryPrincipleId",
                table: "Syllabus",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MorningStatus",
                table: "Attendances",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NightStatus",
                table: "Attendances",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TrainingDeliveryPrinciple",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Marking = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Others = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReTest = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Training = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WaiverCriteria = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingDeliveryPrinciple", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Syllabus_TrainingDeliveryPrincipleId",
                table: "Syllabus",
                column: "TrainingDeliveryPrincipleId",
                unique: true,
                filter: "[TrainingDeliveryPrincipleId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Syllabus_TrainingDeliveryPrinciple_TrainingDeliveryPrincipleId",
                table: "Syllabus",
                column: "TrainingDeliveryPrincipleId",
                principalTable: "TrainingDeliveryPrinciple",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
