SELECT TOP 5
*
FROM
(
	SELECT
	c.[Name]
	,COUNT(r.CategoryId) AS ReportsNumber
	FROM Reports AS r
	JOIN Categories AS c ON r.CategoryId = c.Id
	GROUP BY c.[Name]
) AS a
ORDER BY a.ReportsNumber DESC, a.[Name]
