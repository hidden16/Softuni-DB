SELECT
	CONCAT(c.FirstName, ' ', c.LastName) AS FullName
	,a.Country
	,a.ZIP
	,CONCAT('$',MAX(ci.PriceForSingleCigar)) AS CigarPrice
FROM Addresses AS a
JOIN Clients AS c ON a.Id = c.AddressId
JOIN ClientsCigars AS cc ON c.Id = cc.ClientId
JOIN Cigars AS ci ON cc.CigarId = ci.Id
WHERE ISNUMERIC(a.ZIP) = 1	
GROUP BY c.FirstName,c.LastName,a.Country,a.ZIP