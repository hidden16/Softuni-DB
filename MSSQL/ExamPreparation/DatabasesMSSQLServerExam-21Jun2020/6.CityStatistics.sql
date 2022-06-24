SELECT
*
FROM
(
	SELECT
	c.[Name] AS City
	,COUNT(h.CityId) AS Hotels
	FROM Hotels AS h
	JOIN Cities AS c ON h.CityId = c.Id
	GROUP BY c.[Name]
) AS a
ORDER BY a.Hotels DESC, a.City