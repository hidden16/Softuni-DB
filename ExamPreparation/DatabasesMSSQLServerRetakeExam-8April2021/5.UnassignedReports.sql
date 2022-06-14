SELECT
r.[Description]
,FORMAT(r.OpenDate,'dd-MM-yyyy') AS OpenDate	
FROM Reports AS r
LEFT JOIN Employees AS e ON r.EmployeeId = e.Id
WHERE r.EmployeeId IS NULL
ORDER BY r.OpenDate, r.[Description]