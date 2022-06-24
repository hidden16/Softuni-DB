CREATE FUNCTION udf_ClientWithCigars(@name VARCHAR(30))
RETURNS INT
AS
BEGIN
	DECLARE @cigarsCount INT
	SET @cigarsCount =
		(
			SELECT
			COUNT(c.Id)
			FROM Clients AS c
			JOIN ClientsCigars AS cc ON c.Id = cc.ClientId
			JOIN Cigars AS ci ON cc.CigarId = ci.Id
			WHERE c.FirstName = @name
		)
	RETURN @cigarsCount
END