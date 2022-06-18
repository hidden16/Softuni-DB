SELECT
c.FirstName
,c.Age
,c.PhoneNumber
FROM Customers AS c
JOIN Countries AS cc ON c.CountryId = cc.Id
WHERE (c.Age >= 21 AND c.FirstName LIKE '%an%')
	OR (c.PhoneNumber LIKE '%38' AND cc.[Name] != 'Greece')
ORDER BY c.FirstName, c.Age DESC