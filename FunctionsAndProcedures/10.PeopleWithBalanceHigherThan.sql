CREATE OR ALTER PROCEDURE usp_GetHoldersWithBalanceHigherThan (@number MONEY)
AS
BEGIN
	SELECT
	a.FirstName
	,a.LastName
	FROM AccountHolders AS a
	JOIN Accounts as ac ON a.Id = ac.AccountHolderId
	GROUP BY a.FirstName, a.LastName
	HAVING SUM(ac.Balance) > @number
	ORDER BY a.FirstName, a.LastName
END