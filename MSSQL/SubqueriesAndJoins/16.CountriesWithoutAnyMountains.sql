SELECT
COUNT(c.CountryCode) AS [Count]
FROM
Countries AS c
FULL OUTER JOIN MountainsCountries AS m ON c.CountryCode = m.CountryCode
WHERE m.MountainId IS NULL