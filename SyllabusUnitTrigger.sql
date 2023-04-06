Drop Trigger if exists dbo.trgSyllabusUnitInsert 
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
GO
Drop Trigger if exists dbo.trgSyllabusUnitDelete
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
END
