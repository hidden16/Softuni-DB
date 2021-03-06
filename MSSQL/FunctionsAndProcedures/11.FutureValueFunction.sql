CREATE FUNCTION ufn_CalculateFutureValue(@I DECIMAL(9,2), @R FLOAT, @T INT)
RETURNS DECIMAL(18,4)
AS
BEGIN
	DECLARE @number DECIMAL(18,4)
	SET @number = @I * (POWER(1 + @R,@T))
	RETURN @number
END