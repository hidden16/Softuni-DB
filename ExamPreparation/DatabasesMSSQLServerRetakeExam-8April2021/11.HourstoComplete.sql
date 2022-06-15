CREATE FUNCTION udf_HoursToComplete(@StartDate DATETIME, @EndDate DATETIME)
RETURNS INT
AS
BEGIN
	DECLARE @hours INT
		
	IF(@StartDate IS NULL OR @EndDate IS NULL)
			SET @hours = 0
	ELSE
	BEGIN 
		SET @hours =
			(
			SELECT
			ABS(DATEDIFF(HOUR,@StartDate, @EndDate))
			)
			END
	RETURN @hours
END
