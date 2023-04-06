using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infracstructures.Migrations
{
    /// <inheritdoc />
    public partial class AddLectureUnitTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"DROP TRIGGER IF EXISTS trgLectureInsert
                go 

                CREATE TRIGGER dbo.trgLectureInsert
                ON dbo.Lectures 
                AFTER INSERT AS
                BEGIN	
                    Update dbo.Units 
	                set dbo.Units.Duration = dbo.Units.Duration + (
		                Select top 1 duration
		                FROM inserted
		                Where UnitId = dbo.Units.Id
	                )
	                from dbo.Units
	                Join inserted ON dbo.Units.Id = inserted.UnitId
                END
                Go");

            migrationBuilder.Sql(
                @"DROP TRIGGER IF EXISTS trgLectureDelete
                go
                CREATE TRIGGER dbo.trgLectureDelete
                ON dbo.Lectures 
                FOR DELETE AS
                BEGIN	
                    Update dbo.Units 
	                set dbo.Units.Duration = dbo.Units.Duration - (
		                Select top 1 duration
		                FROM deleted
		                Where UnitId = dbo.Units.Id
	                )
	                from dbo.Units
	                Join deleted ON dbo.Units.Id = deleted.UnitId
                END
                Go");

            migrationBuilder.Sql(
                @"DROP TRIGGER IF EXISTS trgLectureUpdate
                go

                CREATE TRIGGER dbo.trgLectureUpdate
                ON dbo.Lectures 
                AFTER UPDATE AS
                BEGIN	
                    Update dbo.Units 
	                set dbo.Units.Duration = dbo.Units.Duration - 
	                (Select top 1 duration FROM deleted Where UnitId = dbo.Units.Id) + 
	                (Select top 1 duration FROM inserted Where UnitId = dbo.Units.Id)
	                from dbo.Units
	                Join deleted ON dbo.Units.Id = deleted.UnitId
                END
                ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
