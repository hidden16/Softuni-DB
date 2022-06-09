CREATE PROC usp_GetTownsStartingWith(@string VARCHAR(100))
AS
	SELECT
	[Name]
	FROM Towns
	WHERE SUBSTRING([Name],1,LEN(@string)) = @string