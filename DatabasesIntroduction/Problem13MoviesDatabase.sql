CREATE DATABASE [Movies]

CREATE TABLE [Directors](
[Id] INT PRIMARY KEY NOT NULL,
[DirectorName] NVARCHAR(50) NOT NULL,
[Notes] NVARCHAR(MAX)
)
CREATE TABLE [Genres](
[Id] INT PRIMARY KEY NOT NULL,
[GenreName] NVARCHAR(50) NOT NULL,
[Notes] NVARCHAR(MAX)
)
CREATE TABLE [Categories] (
[Id] INT PRIMARY KEY NOT NULL,
[CategoryName] NVARCHAR(50) NOT NULL,
[Notes] NVARCHAR(MAX)
)
CREATE TABLE [Movies](
[Id] INT PRIMARY KEY NOT NULL,
[Title] NVARCHAR(50) NOT NULL,
[DirectorId] INT FOREIGN KEY ([DirectorId]) REFERENCES [Directors]([Id]),
[CopyrightYear] DATE,
[Length] TIME,
[GenreId] INT FOREIGN KEY ([Genreid]) REFERENCES [Genres]([Id]) NOT NULL,
[CategoryId] INT FOREIGN KEY ([CategoryId]) REFERENCES [Categories]([Id]) NOT NULL,
[Rating] TINYINT NOT NULL,
CHECK ([Rating] >= 1 OR [Rating] <= 10),
[Notes] NVARCHAR(MAX)
)

INSERT INTO [Directors]([Id], [DirectorName])
	VALUES
	(1,'Alfred'),
	(2,'Stanley'),
	(3,'Martin'),
	(4,'Akira'),
	(5,'Steven')
	
INSERT INTO [Genres]([Id],[GenreName])
	VALUES
	(1,'Action'),
	(2,'Comedy'),
	(3,'Drama'),
	(4,'Phantasy'),
	(5,'Thriller')

INSERT INTO [Categories]([Id],[CategoryName])
	VALUES
	(1,'ActionMovies'),
	(2,'ComedyMovies'),
	(3,'DramaMovies'),
	(4,'PhantasyMovies'),
	(5,'ThrillerMovies')

INSERT INTO [Movies]([Id],[Title],[DirectorId],[CopyrightYear],[Length],[GenreId],[CategoryId],[Rating])
	VALUES
	(1,'Red Notice', 1, '2021-10-14', '02:40:12',1 ,1 ,8),
	(2,'Ted', 2, '2015-12-21', '01:55:40',2,2,9),
	(3,'Forrest Gump', 3, '1994-06-15', '02:22:10',3,3,7),
	(4,'Avatar', 4, '2006-05-25', '02:10:55',4,4,10),
	(5,'Joker',5, '2018-02-18', '02:05:10',5,5,9)
	