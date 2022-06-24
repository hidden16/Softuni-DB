CREATE FUNCTION ufn_IsWordComprised(@setOfLetters VARCHAR(50), @word VARCHAR(50))
RETURNS BIT
AS
BEGIN
	DECLARE @index INT = 1
	DECLARE @exist BIT = 1
	WHILE(LEN(@word) >= @index AND @exist > 0)
		BEGIN
			DECLARE @charIndex INT
			DECLARE @letter CHAR(1)
			 SET @letter = SUBSTRING(@word,@index,1)
			 SET @charIndex = CHARINDEX(@letter, @setOfLetters)
			 SET @exist =
				CASE
					WHEN @charIndex > 0 THEN 1
					ELSE 0
				END
			SET @index += 1
		END
	RETURN @exist
END
