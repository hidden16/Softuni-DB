CREATE FUNCTION ufn_CashInUsersGames (@gameName VARCHAR(MAX))
RETURNS @table TABLE
(
SumCash MONEY
)
AS
BEGIN
	DECLARE @result MONEY

	SET @result =
		(
		SELECT
		SUM(gg.Cash) AS Cash
		FROM    
			(
			SELECT 
			Cash
			,GameId
			,ROW_NUMBER() OVER (ORDER BY Cash DESC) AS RowNumber
			FROM UsersGames
			WHERE GameId = (SELECT Id FROM Games WHERE Name = @gameName)
			) AS gg
			WHERE gg.RowNumber % 2 != 0
		)
		INSERT INTO @table
		SELECT @result
		RETURN
END