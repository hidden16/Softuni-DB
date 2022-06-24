SELECT TOP 5
cc.CountryName
,r.RiverName
FROM
Continents AS c
RIGHT JOIN Countries AS cc ON c.ContinentCode = cc.ContinentCode
LEFT JOIN CountriesRivers AS cr ON cc.CountryCode = cr.CountryCode
LEFT JOIN Rivers AS r ON r.Id = cr.RiverId
WHERE c.ContinentCode = 'AF'
ORDER BY cc.CountryName