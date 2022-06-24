SELECT
p.FullName
,COUNT(f.PassengerId) AS CountOfAircraft
,SUM(f.TicketPrice) AS TotalPayed
FROM Passengers AS p
JOIN FlightDestinations AS f ON p.Id = f.PassengerId
WHERE
SUBSTRING(p.FullName,2,1) = 'a'
GROUP BY p.FullName
HAVING COUNT(f.PassengerId) > 1
ORDER BY p.FullName