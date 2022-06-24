SELECT
e.EmployeeID 
,e.FirstName 
,CASE WHEN p.StartDate >= '2005' THEN NULL
ELSE p.[Name]
END AS [ProjectName]
FROM
Employees AS e
JOIN EmployeesProjects AS ee ON e.EmployeeID = ee.EmployeeID
JOIN Projects AS p ON ee.ProjectID = p.ProjectID	
WHERE e.EmployeeID = 24