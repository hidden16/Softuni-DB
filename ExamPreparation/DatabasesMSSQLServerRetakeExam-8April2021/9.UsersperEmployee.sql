SELECT
*
FROM
(
SELECT
	CONCAT(e.FirstName, ' ', e.LastName) AS FullName
	,COUNT(u.Id) as UsersCount
	FROM Employees AS e
	LEFT JOIN Reports AS r ON e.Id = r.EmployeeId
	LEFT JOIN Users AS u ON r.UserId = u.Id
	GROUP BY e.FirstName,e.LastName
) AS a
ORDER BY a.UsersCount DESC, a.FullName