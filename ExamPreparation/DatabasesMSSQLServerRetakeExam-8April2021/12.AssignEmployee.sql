SELECT * FROM Employees
SELECT * FROM Categories
SELECT * FROM Reports

CREATE OR ALTER PROCEDURE usp_AssignEmployeeToReport(@EmployeeId INT, @ReportId INT)
AS
BEGIN
	DECLARE @msg VARCHAR(150)
	SET @msg = 'Employee doesn''t belong to the appropriate department!'
	DECLARE @eDepId INT
	SET @eDepId =
	(
		SELECT
		e.DepartmentId
		FROM Employees AS e
		WHERE e.Id = @EmployeeId 
		GROUP BY e.DepartmentId
	)
	DECLARE @cDepId INT
	SET @cDepId =
	(
		SELECT
		c.DepartmentId
		FROM Reports AS r
		JOIN Categories AS c ON r.CategoryId = c.Id
		WHERE r.Id = @ReportId
	)
	IF(@eDepId = @cDepId)
	BEGIN
		UPDATE Reports
		SET EmployeeId = @EmployeeId
	END
	ELSE
		THROW 50001,@msg,1;
END
