DELETE c
FROM Clients AS c
JOIN Addresses AS a ON c.AddressId = a.Id
WHERE SUBSTRING(a.Country,1,1) = 'C'

DELETE FROM Addresses
WHERE SUBSTRING(Country,1,1) = 'C'