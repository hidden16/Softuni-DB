CREATE TABLE [Models](
[ModelID] INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
[Name] VARCHAR(20) NOT NULL,
[ManufacturerID] INT NOT NULL
)

CREATE TABLE [Manufacturers](
[ManufacturerID] INT PRIMARY KEY IDENTITY NOT NULL,
[Name] VARCHAR(20) NOT NULL,
[EstablishedOn] DATE NOT NULL
)

ALTER TABLE [Models]
ADD FOREIGN KEY (ManufacturerID) REFERENCES [Manufacturers](ManufacturerID)

INSERT INTO [Models]([Name],[ManufacturerID])
	VALUES
	('X1',1),
	('i6',1),
	('Model S',2)
	,('Model X',2)
	,('Model 3',2)
	,('Nova',3)

INSERT INTO [Manufacturers]
	VALUES
	('BMW', '1916-03-17')
	,('Tesla', '2003-01-01')
	,('Lada', '1966-05-01')