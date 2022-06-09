CREATE PROC usp_DeleteEmployeesFromDepartment (@departmentId INT)
AS
BEGIN
	DECLARE @EmployeesToDelete TABLE
	(
	Id INT
	)

	INSERT INTO @EmployeesToDelete
	SELECT
	e.EmployeeID
	FROM Employees AS e
	WHERE e.DepartmentID = @departmentId

	ALTER TABLE Departments
	ALTER COLUMN ManagerID INT NULL

	DELETE FROM EmployeesProjects
	WHERE EmployeeID IN (SELECT Id FROM @EmployeesToDelete)

	UPDATE Employees
	SET ManagerID = NULL
	WHERE ManagerID IN (SELECT Id FROM @EmployeesToDelete)

	UPDATE Departments
	SET ManagerID = NULL
	WHERE ManagerID IN (SELECT Id FROM @EmployeesToDelete)

	DELETE FROM Employees
	WHERE EmployeeID IN (SELECT Id FROM @EmployeesToDelete)

	DELETE FROM Departments
	WHERE DepartmentID = @departmentId

	SELECT
	COUNT(*) AS [Employees Count]
	FROM Employees AS e
	JOIN Departments AS d ON e.DepartmentID = d.DepartmentID
	WHERE d.DepartmentID = @departmentId

END