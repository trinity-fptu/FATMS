using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infracstructures.Migrations
{
    /// <inheritdoc />
    public partial class AddSyllabusTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"Drop Trigger if exists dbo.trgSyllabusUnitInsert 
                go
                CREATE TRIGGER dbo.trgSyllabusUnitInsert
                ON dbo.SyllabusUnit
                AFTER INSERT AS
                BEGIN
	                Update dbo.Syllabus 
	                set dbo.Syllabus.TimeDuration = dbo.Syllabus.TimeDuration + (
		                Select sum (duration)
		                FROM dbo.Units join  inserted on dbo.Units.Id = inserted.UnitsId
		                Where inserted.UnitsId = dbo.Units.Id
	                )
	                from dbo.Syllabus
	                Join inserted ON dbo.Syllabus.Id = inserted.SyllabusesId
                END
                GO");

            migrationBuilder.Sql(
                @"Drop Trigger if exists dbo.trgSyllabusUnitDelete
                go
                CREATE TRIGGER dbo.trgSyllabusUnitDelete
                ON dbo.SyllabusUnit
                For Delete AS
                BEGIN
	                Update dbo.Syllabus 
	                set dbo.Syllabus.TimeDuration = dbo.Syllabus.TimeDuration - (
		                Select duration
		                FROM dbo.Units join  deleted on dbo.Units.Id = deleted.UnitsId
		                Where deleted.UnitsId = dbo.Units.Id
	                )
	                from dbo.Syllabus
	                Join deleted ON dbo.Syllabus.Id = deleted.SyllabusesId

	                select * from deleted
                END");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OperationDate",
                table: "ClassUnitDetail",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "Location",
                table: "ClassUnitDetail",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DayNo",
                table: "ClassUnitDetail",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "OperationDate",
                table: "ClassUnitDetail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Location",
                table: "ClassUnitDetail",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DayNo",
                table: "ClassUnitDetail",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
