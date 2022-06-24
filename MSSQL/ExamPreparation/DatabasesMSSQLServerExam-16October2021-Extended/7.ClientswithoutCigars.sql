SELECT * FROM Cigars
SELECT * FROM Clients
SELECT *FROM ClientsCigars

SELECT 
*
FROM
(
	SELECT
	c.Id
	,CONCAT(c.FirstName, ' ', c.LastName) AS ClientName
	,c.Email
	FROM Clients AS c
	LEFT JOIN ClientsCigars AS cc ON c.Id = cc.ClientId
	WHERE cc.CigarId IS NULL
) AS a
ORDER BY ClientName 