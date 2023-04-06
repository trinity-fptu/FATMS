Drop trigger if exists trgSyllabusUnitInsert
Drop trigger if exists trgSyllabusUnitDelete
Drop trigger if exists trgUnitUpdate
go

CREATE TRIGGER dbo.trgSyllabusUnitInsert
ON dbo.SyllabusUnit
AFTER INSERT AS
DECLARE @MaxDay INT
SELECT @MaxDay = (SELECT MAX(u.Session)
                  FROM inserted, Units u, Syllabus s, SyllabusUnit su
				  WHERE su.SyllabusesId = s.Id and su.UnitsId = u.Id and su.SyllabusesId = inserted.SyllabusesId)
DECLARE @TotalHours INT 
SELECT @TotalHours = (SELECT Sum(u.Duration)
                      FROM inserted, Units u, Syllabus s, SyllabusUnit su
				      WHERE su.SyllabusesId = s.Id and su.UnitsId = u.Id and su.SyllabusesId = inserted.SyllabusesId)/60
BEGIN
    Update dbo.Syllabus
	set dbo.Syllabus.DaysDuration = @MaxDay, dbo.Syllabus.TimeDuration = @TotalHours
	FROM dbo.Syllabus, inserted
	WHERE dbo.Syllabus.Id = inserted.SyllabusesId
END
Go

CREATE TRIGGER dbo.trgSyllabusUnitDelete
ON dbo.SyllabusUnit
AFTER DELETE AS
DECLARE @MaxDay INT
SELECT @MaxDay = (SELECT MAX(u.Session)
                  FROM deleted, Units u, Syllabus s, SyllabusUnit su
				  WHERE su.SyllabusesId = s.Id and su.UnitsId = u.Id and su.SyllabusesId = deleted.SyllabusesId)
DECLARE @TotalHours INT 
SELECT @TotalHours = (SELECT Sum(u.Duration)
                      FROM deleted, Units u, Syllabus s, SyllabusUnit su
				      WHERE su.SyllabusesId = s.Id and su.UnitsId = u.Id and su.SyllabusesId = deleted.SyllabusesId)/60
BEGIN
    Update dbo.Syllabus
	set dbo.Syllabus.DaysDuration = @MaxDay, dbo.Syllabus.TimeDuration = @TotalHours
	FROM dbo.Syllabus, deleted
	WHERE dbo.Syllabus.Id = deleted.SyllabusesId
END
Go

CREATE TRIGGER dbo.trgUnitUpdate
ON dbo.Units
AFTER UPDATE AS
DECLARE @MaxDay INT
SELECT @MaxDay = (Select Max(u.Session)
                 FROM SyllabusUnit su, Syllabus s, Units u
				 WHERE su.SyllabusesId = s.Id and su.UnitsId = u.Id and su.SyllabusesId = (SELECT SyllabusUnit.SyllabusesId
																						   FROM inserted, SyllabusUnit
																						   WHERE inserted.Id = SyllabusUnit.UnitsId))
DECLARE @TotalHours INT 
SELECT @TotalHours = (Select Sum(u.Duration)
                      FROM SyllabusUnit su, Syllabus s, Units u
				      WHERE su.SyllabusesId = s.Id and su.UnitsId = u.Id and su.SyllabusesId = (SELECT SyllabusUnit.SyllabusesId
																						        FROM inserted, SyllabusUnit
																						        WHERE inserted.Id = SyllabusUnit.UnitsId))/60
BEGIN
    Update dbo.Syllabus
	set dbo.Syllabus.DaysDuration = @MaxDay, dbo.Syllabus.TimeDuration = @TotalHours
	FROM dbo.Syllabus, dbo.SyllabusUnit, inserted
	WHERE dbo.SyllabusUnit.UnitsId = inserted.Id and dbo.Syllabus.Id =  SyllabusUnit.SyllabusesId
END
Go