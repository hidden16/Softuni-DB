INSERT INTO Passengers
SELECT
CONCAT(FirstName, ' ', LastName) AS FullName
,CONCAT(FirstName,LastName,'@gmail.com')
FROM Pilots AS p
WHERE p.Id BETWEEN 5 AND 15