SELECT
a.DepartmentID
,a.Salary
FROM
(
SELECT
DepartmentID
,Salary
,DENSE_RANK() OVER (PARTITION BY DepartmentID ORDER BY Salary DESC) AS [Rank]
FROM Employees
) AS a
WHERE a.[Rank] = 3
GROUP BY a.DepartmentID, a.Salary, a.[Rank]
ORDER BY DepartmentID, Salary DESC
