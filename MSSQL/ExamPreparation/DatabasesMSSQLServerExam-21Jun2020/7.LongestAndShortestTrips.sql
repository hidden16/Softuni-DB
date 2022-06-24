SELECT
a.Id AS AccountId
,CONCAT(a.FirstName, ' ', a.LastName) AS FullName
,ABS(MAX(DATEDIFF(DAY,t.ArrivalDate,t.ReturnDate))) AS LongestTrip
,ABS(MIN(DATEDIFF(DAY,t.ArrivalDate,t.ReturnDate))) AS ShortestTrip
FROM Accounts AS a
JOIN AccountsTrips AS aa ON a.Id = aa.AccountId
JOIN Trips AS t ON aa.TripId = t.Id
WHERE a.MiddleName IS NULL
	AND t.CancelDate IS NULL
GROUP BY a.Id,a.FirstName,a.LastName
ORDER BY LongestTrip DESC, ShortestTrip ASC