using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infracstructures.Migrations
{
    /// <inheritdoc />
    public partial class initDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OutputStandards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutputStandards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingDeliveryPrinciple",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Training = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReTest = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Marking = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WaiverCriteria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Others = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingDeliveryPrinciple", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Session = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    IsMale = table.Column<bool>(type: "bit", nullable: false),
                    AvatarURL = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lectures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    LessonType = table.Column<int>(type: "int", nullable: false),
                    DeliveryType = table.Column<int>(type: "int", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: true),
                    OutputStandardId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lectures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lectures_OutputStandards_OutputStandardId",
                        column: x => x.OutputStandardId,
                        principalTable: "OutputStandards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Lectures_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "FeedBackForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    TraineeId = table.Column<int>(type: "int", nullable: true),
                    TrainerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedBackForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedBackForms_Users_TraineeId",
                        column: x => x.TraineeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedBackForms_Users_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Syllabus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Version = table.Column<float>(type: "real", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<int>(type: "int", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    AttendeeNumber = table.Column<int>(type: "int", nullable: false),
                    CourseObjectives = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrainingDeliveryPrincipleId = table.Column<int>(type: "int", nullable: true),
                    QuizCriteria = table.Column<float>(type: "real", nullable: false),
                    AssignmentCriteria = table.Column<float>(type: "real", nullable: false),
                    FinalCriteria = table.Column<float>(type: "real", nullable: false),
                    FinalTheoryCriteria = table.Column<float>(type: "real", nullable: false),
                    FinalPracticalCriteria = table.Column<float>(type: "real", nullable: false),
                    PassingGPA = table.Column<float>(type: "real", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Syllabus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Syllabus_TrainingDeliveryPrinciple_TrainingDeliveryPrincipleId",
                        column: x => x.TrainingDeliveryPrincipleId,
                        principalTable: "TrainingDeliveryPrinciple",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Syllabus_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Syllabus_Users_LastModifiedBy",
                        column: x => x.LastModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TMS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CheckedBy = table.Column<int>(type: "int", nullable: true),
                    TraineeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TMS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TMS_Users_CheckedBy",
                        column: x => x.CheckedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TMS_Users_TraineeId",
                        column: x => x.TraineeId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingPrograms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    LastModify = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastModifyBy = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingPrograms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingPrograms_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrainingPrograms_Users_LastModifyBy",
                        column: x => x.LastModifyBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GradeReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    GradedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Grade = table.Column<float>(type: "real", nullable: false),
                    TraineeId = table.Column<int>(type: "int", nullable: false),
                    LectureId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradeReports_Lectures_LectureId",
                        column: x => x.LectureId,
                        principalTable: "Lectures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradeReports_Users_TraineeId",
                        column: x => x.TraineeId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TrainingMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LectureId = table.Column<int>(type: "int", nullable: false),
                    EditedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingMaterials_Lectures_LectureId",
                        column: x => x.LectureId,
                        principalTable: "Lectures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingMaterials_Users_EditedBy",
                        column: x => x.EditedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlannedBy = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SyllabusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditPlans_Syllabus_SyllabusId",
                        column: x => x.SyllabusId,
                        principalTable: "Syllabus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditPlans_Users_PlannedBy",
                        column: x => x.PlannedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SyllabusUnit",
                columns: table => new
                {
                    SyllabusesId = table.Column<int>(type: "int", nullable: false),
                    UnitsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyllabusUnit", x => new { x.SyllabusesId, x.UnitsId });
                    table.ForeignKey(
                        name: "FK_SyllabusUnit_Syllabus_SyllabusesId",
                        column: x => x.SyllabusesId,
                        principalTable: "Syllabus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SyllabusUnit_Units_UnitsId",
                        column: x => x.UnitsId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Class",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<int>(type: "int", nullable: true),
                    AttendeeType = table.Column<int>(type: "int", nullable: true),
                    FSU = table.Column<int>(type: "int", nullable: true),
                    ClassTime = table.Column<int>(type: "int", nullable: false),
                    StartedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinishedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ApprovedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApprovedBy = table.Column<int>(type: "int", nullable: true),
                    TrainingProgramId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Class", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Class_TrainingPrograms_TrainingProgramId",
                        column: x => x.TrainingProgramId,
                        principalTable: "TrainingPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Class_Users_ApprovedBy",
                        column: x => x.ApprovedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Class_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SyllabusTrainingProgram",
                columns: table => new
                {
                    SyllabusesId = table.Column<int>(type: "int", nullable: false),
                    TrainingProgramsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyllabusTrainingProgram", x => new { x.SyllabusesId, x.TrainingProgramsId });
                    table.ForeignKey(
                        name: "FK_SyllabusTrainingProgram_Syllabus_SyllabusesId",
                        column: x => x.SyllabusesId,
                        principalTable: "Syllabus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SyllabusTrainingProgram_TrainingPrograms_TrainingProgramsId",
                        column: x => x.TrainingProgramsId,
                        principalTable: "TrainingPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Feedback = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    AuditorId = table.Column<int>(type: "int", nullable: false),
                    TraineeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditDetails_AuditPlans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "AuditPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditDetails_Users_AuditorId",
                        column: x => x.AuditorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AuditDetails_Users_TraineeId",
                        column: x => x.TraineeId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClassUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassUsers_Class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    ClassUserId = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MorningStatus = table.Column<int>(type: "int", nullable: true),
                    NoonStatus = table.Column<int>(type: "int", nullable: true),
                    NightStatus = table.Column<int>(type: "int", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => new { x.ClassUserId, x.Day });
                    table.ForeignKey(
                        name: "FK_Attendances_ClassUsers_ClassUserId",
                        column: x => x.ClassUserId,
                        principalTable: "ClassUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditDetails_AuditorId",
                table: "AuditDetails",
                column: "AuditorId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditDetails_PlanId",
                table: "AuditDetails",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditDetails_TraineeId",
                table: "AuditDetails",
                column: "TraineeId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditPlans_PlannedBy",
                table: "AuditPlans",
                column: "PlannedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AuditPlans_SyllabusId",
                table: "AuditPlans",
                column: "SyllabusId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Class_TrainingProgramId",
                table: "Class",
                column: "TrainingProgramId",
                unique: true,
                filter: "[TrainingProgramId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClassUsers_ClassId",
                table: "ClassUsers",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassUsers_UserId",
                table: "ClassUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedBackForms_TraineeId",
                table: "FeedBackForms",
                column: "TraineeId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedBackForms_TrainerId",
                table: "FeedBackForms",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeReports_LectureId",
                table: "GradeReports",
                column: "LectureId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeReports_TraineeId",
                table: "GradeReports",
                column: "TraineeId");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_OutputStandardId",
                table: "Lectures",
                column: "OutputStandardId",
                unique: true,
                filter: "[OutputStandardId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Lectures_UnitId",
                table: "Lectures",
                column: "UnitId");

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
                name: "IX_Syllabus_TrainingDeliveryPrincipleId",
                table: "Syllabus",
                column: "TrainingDeliveryPrincipleId",
                unique: true,
                filter: "[TrainingDeliveryPrincipleId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SyllabusTrainingProgram_TrainingProgramsId",
                table: "SyllabusTrainingProgram",
                column: "TrainingProgramsId");

            migrationBuilder.CreateIndex(
                name: "IX_SyllabusUnit_UnitsId",
                table: "SyllabusUnit",
                column: "UnitsId");

            migrationBuilder.CreateIndex(
                name: "IX_TMS_CheckedBy",
                table: "TMS",
                column: "CheckedBy",
                unique: true,
                filter: "[CheckedBy] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TMS_TraineeId",
                table: "TMS",
                column: "TraineeId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingMaterials_EditedBy",
                table: "TrainingMaterials",
                column: "EditedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingMaterials_LectureId",
                table: "TrainingMaterials",
                column: "LectureId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "AuditDetails");

            migrationBuilder.DropTable(
                name: "FeedBackForms");

            migrationBuilder.DropTable(
                name: "GradeReports");

            migrationBuilder.DropTable(
                name: "SyllabusTrainingProgram");

            migrationBuilder.DropTable(
                name: "SyllabusUnit");

            migrationBuilder.DropTable(
                name: "TMS");

            migrationBuilder.DropTable(
                name: "TrainingMaterials");

            migrationBuilder.DropTable(
                name: "ClassUsers");

            migrationBuilder.DropTable(
                name: "AuditPlans");

            migrationBuilder.DropTable(
                name: "Lectures");

            migrationBuilder.DropTable(
                name: "Class");

            migrationBuilder.DropTable(
                name: "Syllabus");

            migrationBuilder.DropTable(
                name: "OutputStandards");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "TrainingPrograms");

            migrationBuilder.DropTable(
                name: "TrainingDeliveryPrinciple");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}