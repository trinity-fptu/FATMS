using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infracstructures.Migrations
{
    /// <inheritdoc />
    public partial class AuditUpdatDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditDetails");

            migrationBuilder.DropTable(
                name: "AuditPlans");

            migrationBuilder.CreateTable(
                name: "AuditPlan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlannedBy = table.Column<int>(type: "int", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SyllabusId = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditPlan_Class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditPlan_Syllabus_SyllabusId",
                        column: x => x.SyllabusId,
                        principalTable: "Syllabus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditPlan_Users_PlannedBy",
                        column: x => x.PlannedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AuditDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Feedback = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    AuditorId = table.Column<int>(type: "int", nullable: true),
                    TraineeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditDetail_AuditPlan_PlanId",
                        column: x => x.PlanId,
                        principalTable: "AuditPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditDetail_Users_AuditorId",
                        column: x => x.AuditorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AuditDetail_Users_TraineeId",
                        column: x => x.TraineeId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AuditResult",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TraineeAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    AuditDetailId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditResult_AuditDetail_AuditDetailId",
                        column: x => x.AuditDetailId,
                        principalTable: "AuditDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditDetail_AuditorId",
                table: "AuditDetail",
                column: "AuditorId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditDetail_PlanId",
                table: "AuditDetail",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditDetail_TraineeId",
                table: "AuditDetail",
                column: "TraineeId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditPlan_ClassId",
                table: "AuditPlan",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditPlan_PlannedBy",
                table: "AuditPlan",
                column: "PlannedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AuditPlan_SyllabusId",
                table: "AuditPlan",
                column: "SyllabusId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditResult_AuditDetailId",
                table: "AuditResult",
                column: "AuditDetailId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditResult");

            migrationBuilder.DropTable(
                name: "AuditDetail");

            migrationBuilder.DropTable(
                name: "AuditPlan");

            migrationBuilder.CreateTable(
                name: "AuditPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlannedBy = table.Column<int>(type: "int", nullable: false),
                    SyllabusId = table.Column<int>(type: "int", nullable: false),
                    AuditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
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
                name: "AuditDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditorId = table.Column<int>(type: "int", nullable: false),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    TraineeId = table.Column<int>(type: "int", nullable: false),
                    Feedback = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false)
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
        }
    }
}
