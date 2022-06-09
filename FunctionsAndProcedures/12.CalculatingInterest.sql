CREATE PROCEDURE usp_CalculateFutureValueForAccount (@accountId INT, @interestRate FLOAT)
AS	SELECT 
	@accountId AS [Account Id]
	,a.FirstName AS [First Name]
	,a.LastName AS [Last Name]
	,aa.Balance AS [Current Balance]
	,dbo.ufn_CalculateFutureValue(aa.Balance,@interestRate,5) AS [Balance in 5 years]
	FROM AccountHolders AS a
	JOIN Accounts AS aa ON a.Id = aa.Id
	WHERE a.Id = @accountId