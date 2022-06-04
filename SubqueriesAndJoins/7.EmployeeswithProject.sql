SELECT TOP 5
e.EmployeeID
,e.FirstName
,p.[Name] AS [ProjectName]
FROM
Employees AS e
JOIN EmployeesProjects AS ee ON e.EmployeeID = ee.EmployeeID
JOIN Projects AS p ON ee.ProjectID = p.ProjectID
WHERE p.EndDate IS NULL

