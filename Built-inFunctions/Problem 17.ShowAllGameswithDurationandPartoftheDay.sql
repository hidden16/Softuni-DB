SELECT
[Name] AS Game,
[Start] AS [Part of the Day],
[Duration]
FROM
(
	SELECT
		[Name]
		,[Start]  = 
	CASE 
		WHEN DATEPART(HOUR,[Start]) BETWEEN 0 AND 11 THEN 'Morning'
		WHEN DATEPART(HOUR,[Start]) BETWEEN 12 AND 17 THEN 'Afternoon'
		WHEN DATEPART(HOUR,[Start]) BETWEEN 18 AND 24 THEN 'Evening'
	END
	,[Duration] =
	CASE	
		WHEN [Duration] <= 3 THEN 'Extra Short'
		WHEN [Duration] BETWEEN 4 AND 6 THEN 'Short'
		WHEN [Duration] >= 6 THEN 'Long'
		WHEN [Duration] IS NULL THEN 'Extra Long'
	END
	FROM Games
)
AS [Inner]
ORDER BY [Name], [Duration], [Part of the Day]
