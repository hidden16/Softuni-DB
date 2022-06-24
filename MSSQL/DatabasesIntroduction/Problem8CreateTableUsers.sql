CREATE TABLE [Users] (
[Id] BIGINT PRIMARY KEY IDENTITY,
[Username] VARCHAR(30) NOT NULL,
[Password] VARCHAR(26) NOT NULL,
[ProfilePicture] VARBINARY,
CHECK (DATALENGTH ([ProfilePicture]) <= 900000),
[LastLoginTime] DATETIME2,
[IsDeleted] CHAR(1),
CHECK ([IsDeleted] = '0' OR [IsDeleted] = '1')
)

INSERT INTO [Users]([Username], [Password], [LastLoginTime], [IsDeleted])
	VALUES
	('KOLooper', 'kekesss22211', '2022-5-18 16:42:52', '0'),
	('Techies', 'BombsEverywhere11', NULL, '1'),
	('Garen', 'Demacia', '2022-5-20 15:06:22', '0'),
	('Necromancer', 'SkeletonsAreMyFavorite', NULL, '0'),
	('Bristleback', 'SomeHeavyThorns', NULL, '1')