CREATE VIEW V_EmployeeNameJobTitle AS
SELECT CONCAT([FirstName],' ',COALESCE([MiddleName], ''),' ',[LastName]) AS "Full Name", [JobTitle]
FROM [Employees]