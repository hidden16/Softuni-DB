CREATE TABLE NotificationEmails(
Id INT PRIMARY KEY IDENTITY NOT NULL
,Recipient INT NOT NULL
,[Subject] NVARCHAR(50) NOT NULL
,Body NVARCHAR(150) NOT NULL
)
GO

CREATE TRIGGER tr_AddToNotificationEmailsEveryTimeLogsUpdates
ON Logs AFTER INSERT
AS
	INSERT INTO NotificationEmails(Recipient,[Subject],Body)
	SELECT
	l.AccountId
	,CONCAT('Balance change for account: ', l.AccountId)
	,CONCAT('On ',GETDATE(),' your balance was changed from ', l.OldSum, ' to ', l.NewSum, '.')
	FROM Logs AS l