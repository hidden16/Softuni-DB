SELECT
a.AirportName
,f.[Start]
,f.TicketPrice
,p.FullName
,aa.Manufacturer
,aa.Model
FROM FlightDestinations AS f
JOIN Airports AS a ON f.AirportId = a.Id
JOIN Passengers AS p ON f.PassengerId = p.Id
JOIN Aircraft AS aa ON f.AircraftId = aa.Id
WHERE DATEPART(HOUR, f.[Start]) BETWEEN 6 AND 20
AND f.TicketPrice > 2500
ORDER BY aa.Model ASC