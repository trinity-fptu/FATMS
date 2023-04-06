Drop trigger if exists trgLectureInsert
Drop trigger if exists trgLectureDelete
Drop trigger if exists trgLectureUpdate
go
CREATE TRIGGER dbo.trgLectureInsert
ON dbo.Lectures 
AFTER INSERT AS
BEGIN	
    Update dbo.Units 
	set dbo.Units.Duration = dbo.Units.Duration + (
		Select duration
		FROM inserted
		Where UnitId = dbo.Units.Id
	)
	from dbo.Units
	Join inserted ON dbo.Units.Id = inserted.UnitId
END
Go

CREATE TRIGGER dbo.trgLectureDelete
ON dbo.Lectures 
FOR DELETE AS
BEGIN	
    Update dbo.Units 
	set dbo.Units.Duration = dbo.Units.Duration - (
		Select duration
		FROM deleted
		Where UnitId = dbo.Units.Id
	)
	from dbo.Units
	Join deleted ON dbo.Units.Id = deleted.UnitId
END
Go

CREATE TRIGGER dbo.trgLectureUpdate
ON dbo.Lectures 
AFTER UPDATE AS
BEGIN	
    Update dbo.Units 
	set dbo.Units.Duration = dbo.Units.Duration - 
	(Select duration FROM deleted Where UnitId = dbo.Units.Id) + 
	(Select duration FROM inserted Where UnitId = dbo.Units.Id)
	from dbo.Units
	Join deleted ON dbo.Units.Id = deleted.UnitId
END
