Drop Trigger if exists dbo.trgSyllabusTrainingProgramInsert 
go
CREATE TRIGGER dbo.trgSyllabusTrainingProgramInsert
ON dbo.SyllabusTrainingProgram
AFTER INSERT AS
BEGIN
	Update dbo.TrainingPrograms 
	set dbo.TrainingPrograms.TimeDuration = dbo.TrainingPrograms.TimeDuration + (
		Select SUM(Syllabus.timeDuration)
		FROM dbo.Syllabus join  inserted on dbo.Syllabus.Id = inserted.SyllabusesId
		Where inserted.SyllabusesId = dbo.Syllabus.Id
	), dbo.TrainingPrograms.DaysDuration = dbo.TrainingPrograms.DaysDuration + (
		Select SUM(Syllabus.DaysDuration)
		FROM dbo.Syllabus join  inserted on dbo.Syllabus.Id = inserted.SyllabusesId
		Where inserted.SyllabusesId = dbo.Syllabus.Id
	)
	from dbo.TrainingPrograms
	Join inserted ON dbo.TrainingPrograms.Id = inserted.TrainingProgramsId
END
GO
Drop Trigger if exists dbo.trgSyllabusTrainingProgramDelete
go
CREATE TRIGGER dbo.trgSyllabusTrainingProgramDelete
ON dbo.SyllabusTrainingProgram
AFTER Delete AS
BEGIN
	Update dbo.TrainingPrograms 
	set dbo.TrainingPrograms.TimeDuration = dbo.TrainingPrograms.TimeDuration - (
		Select SUM( timeDuration)
		FROM dbo.Syllabus join  deleted on dbo.Syllabus.Id = deleted.SyllabusesId
		Where deleted.SyllabusesId = dbo.Syllabus.Id
	),dbo.TrainingPrograms.DaysDuration = dbo.TrainingPrograms.DaysDuration - (
		Select SUm( Syllabus.DaysDuration)
		FROM dbo.Syllabus join  deleted on dbo.Syllabus.Id = deleted.SyllabusesId
		Where deleted.SyllabusesId = dbo.Syllabus.Id
	)
	from dbo.TrainingPrograms
	Join deleted ON dbo.TrainingPrograms.Id = deleted.TrainingProgramsId
END
GO
