CREATE TABLE Logs(
Id INT PRIMARY KEY IDENTITY NOT NULL
,AccountId INT NOT NULL
,OldSum INT NOT NULL
,NewSum INT NOT NULL
)
GO

CREATE TRIGGER tr_AddToLogsEveryTimeSumChanges
ON Accounts AFTER UPDATE
AS
	INSERT INTO Logs(AccountId,OldSum,NewSum)
	SELECT
	i.Id,d.Balance,i.Balance
	FROM INSERTED AS i
	JOIN DELETED AS d ON i.Id = d.Id
	WHERE i.Balance != d.Balance
GO