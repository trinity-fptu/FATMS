Drop trigger if exists trgAuditResultInsert
go

CREATE TRIGGER dbo.trgAuditResultInsert
ON dbo.AuditResult
AFTER INSERT AS
DECLARE @NumOfQuestion INT, @OverallRating INT, @MaxRating INT, @Average FLOAT, @Status BIT
SELECT @NumOfQuestion = (SELECT COUNT(*)
                         FROM AuditResult
						 JOIN inserted ON dbo.AuditResult.AuditDetailId = inserted.AuditDetailId)
SELECT @MaxRating = @NumOfQuestion * 3
SELECT @OverallRating = (SELECT SUM(AuditResult.Rating)
                         FROM AuditResult
						 JOIN inserted ON dbo.AuditResult.AuditDetailId = inserted.AuditDetailId)
SELECT @Average = CAST(@OverallRating AS FLOAT) / CAST(@MaxRating AS FLOAT)
IF (@Average * 100) >= 50
BEGIN
set @Status = 1
END
ELSE IF (@Average * 100) < 50
BEGIN
set @Status = 0
END
BEGIN	
    Update dbo.AuditDetail 
	set dbo.AuditDetail.Status = @Status
	FROM dbo.AuditDetail
	JOIN inserted ON dbo.AuditDetail.Id = inserted.AuditDetailId
END
