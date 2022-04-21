ALTER TABLE [Users]
	DROP CONSTRAINT PK__Users__77222459FF443B32
GO
ALTER TABLE [Users]
	ADD CONSTRAINT PK__Users__Id
	PRIMARY KEY ([Id])
GO
ALTER TABLE [Users]
	ADD CONSTRAINT UQ__Users__Username
	UNIQUE ([Username]), CHECK(LEN([Username]) >= 3 )