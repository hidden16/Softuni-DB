SELECT
a.Id
,a.Email
,c.[Name]
,COUNT(*) AS Trips
FROM AccountsTrips AS [at]
JOIN Trips AS t ON [at].TripId = t.Id
JOIN Rooms AS r ON t.RoomId = r.Id
JOIN Hotels AS h ON r.HotelId = h.Id
JOIN Cities AS c ON h.CityId = c.Id
JOIN Accounts AS a ON c.Id = a.CityId
					AND a.Id = [at].AccountId
GROUP BY a.Id,a.Email,c.[Name]
ORDER BY Trips DESC,a.Id