SELECT
MIN(a.AVGSalary) AS MinAverageSalary
FROM
(
	SELECT 
	e.DepartmentID
	,AVG(e.Salary) AS AVGSalary
	FROM
	Employees AS e
	GROUP BY e.DepartmentID
) AS a