SELECT TOP 10
*
FROM
(
	SELECT
	c.Id
	,c.[Name] AS City
	,c.CountryCode AS Country
	,COUNT(*) AS Accounts
	FROM Accounts AS a
	JOIN Cities AS c ON a.CityId = c.Id
	GROUP BY c.Id,c.[Name],c.CountryCode
) AS a
ORDER BY a.Accounts DESC