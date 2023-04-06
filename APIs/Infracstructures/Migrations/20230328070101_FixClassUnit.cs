using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infracstructures.Migrations
{
    /// <inheritdoc />
    public partial class FixClassUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassUnitDetail_Users_TrainerId",
                table: "ClassUnitDetail");

            migrationBuilder.AlterColumn<int>(
                name: "TrainerId",
                table: "ClassUnitDetail",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassUnitDetail_Users_TrainerId",
                table: "ClassUnitDetail",
                column: "TrainerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassUnitDetail_Users_TrainerId",
                table: "ClassUnitDetail");

            migrationBuilder.AlterColumn<int>(
                name: "TrainerId",
                table: "ClassUnitDetail",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassUnitDetail_Users_TrainerId",
                table: "ClassUnitDetail",
                column: "TrainerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
