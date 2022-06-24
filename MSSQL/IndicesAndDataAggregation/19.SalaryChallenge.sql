SELECT  TOP 10
FirstName
,LastName
,DepartmentID
FROM Employees AS e
WHERE e.Salary > (
					SELECT
					AVG(Salary) AS AverageSalary
					FROM Employees AS es
					WHERE es.DepartmentID = e.DepartmentID
					GROUP BY DepartmentID
				 )
ORDER BY DepartmentID