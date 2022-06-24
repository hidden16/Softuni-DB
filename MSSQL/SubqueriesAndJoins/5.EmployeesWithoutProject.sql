SELECT TOP 3
e.EmployeeID
,e.FirstName
FROM
Employees AS e
FULL OUTER JOIN EmployeesProjects AS ee ON e.EmployeeID = ee.EmployeeID
WHERE ee.EmployeeID IS NULL