using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infracstructures.Migrations
{
    /// <inheritdoc />
    public partial class AddQuizRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quiz",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    QuizStatus = table.Column<int>(type: "int", nullable: false),
                    TimeLimit = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateTrainerId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quiz", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quiz_Class_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Class",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Quiz_Users_CreateTrainerId",
                        column: x => x.CreateTrainerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizBank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    Grade = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizBank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizBank_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizBank_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuizDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuizId = table.Column<int>(type: "int", nullable: false),
                    QuizBankId = table.Column<int>(type: "int", nullable: false),
                    Grade = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizDetail_QuizBank_QuizBankId",
                        column: x => x.QuizBankId,
                        principalTable: "QuizBank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizDetail_Quiz_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quiz",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizRecord",
                columns: table => new
                {
                    QuizDetailId = table.Column<int>(type: "int", nullable: false),
                    TraineeId = table.Column<int>(type: "int", nullable: false),
                    TraineeAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Grade = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizRecord", x => new { x.TraineeId, x.QuizDetailId });
                    table.ForeignKey(
                        name: "FK_QuizRecord_QuizDetail_QuizDetailId",
                        column: x => x.QuizDetailId,
                        principalTable: "QuizDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizRecord_Users_TraineeId",
                        column: x => x.TraineeId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Quiz_ClassId",
                table: "Quiz",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Quiz_CreateTrainerId",
                table: "Quiz",
                column: "CreateTrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizBank_CreatedBy",
                table: "QuizBank",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_QuizBank_UnitId",
                table: "QuizBank",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizDetail_QuizBankId",
                table: "QuizDetail",
                column: "QuizBankId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizDetail_QuizId",
                table: "QuizDetail",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizRecord_QuizDetailId",
                table: "QuizRecord",
                column: "QuizDetailId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuizRecord");

            migrationBuilder.DropTable(
                name: "QuizDetail");

            migrationBuilder.DropTable(
                name: "QuizBank");

            migrationBuilder.DropTable(
                name: "Quiz");
        }
    }
}
