SELECT
c.CountryCode
,COUNT(mm.MountainRange) AS [MountainRanges]
FROM
Countries AS c
JOIN MountainsCountries AS m ON c.CountryCode = m.CountryCode
JOIN Mountains as mm ON m.MountainId = mm.Id
WHERE c.CountryCode = 'BG'
	OR c.CountryCode = 'RU'
	OR c.CountryCode = 'US'
	GROUP BY c.CountryCode