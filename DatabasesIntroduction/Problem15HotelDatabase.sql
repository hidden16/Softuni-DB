CREATE DATABASE [Hotel]
USE [Hotel]

CREATE TABLE [Employees](
[Id] INT PRIMARY KEY NOT NULL,
[FirstName] NVARCHAR(50) NOT NULL,
[Title] NVARCHAR(30) NOT NULL,
[Notes] NVARCHAR(MAX)
)

CREATE TABLE [Customers](
[AccountNumber] INT PRIMARY KEY NOT NULL,
[FirstName] NVARCHAR(50) NOT NULL,
[LastName] NVARCHAR(50) NOT NULL,
[PhoneNumber] INT NOT NULL,
[EmergencyName] NVARCHAR(50) NOT NULL,
[EmergencyNumber] INT NOT NULL,
[Notes] NVARCHAR(MAX)
)

CREATE TABLE [RoomStatus](
[RoomStatus] VARCHAR(10) PRIMARY KEY NOT NULL,
CHECK ([RoomStatus] = 'Free' OR [RoomStatus] = 'Taken' OR [RoomStatus] = 'Reserved'),
[Notes] NVARCHAR(MAX)
)

CREATE TABLE [RoomTypes](
[RoomType] VARCHAR(30) PRIMARY KEY NOT NULL,
CHECK ([RoomType] = 'Single' OR [RoomType] = 'Double' OR [RoomType] = 'Triple' OR [RoomType] = 'Quad'),
[Notes] NVARCHAR(MAX)
)

CREATE TABLE [BedTypes](
[BedType] VARCHAR(20) PRIMARY KEY NOT NULL,
CHECK ([BedType] = 'Small' OR [BedType] = 'Medium' OR [BedType] = 'Big'),
[Notes] NVARCHAR(MAX)
)

CREATE TABLE [Rooms](
[RoomNumber] INT PRIMARY KEY NOT NULL,
[RoomType] VARCHAR(30) FOREIGN KEY ([RoomType]) REFERENCES [RoomTypes]([RoomType]) NOT NULL,
[BedType] VARCHAR(20) FOREIGN KEY ([BedType]) REFERENCES [BedTypes]([BedType]) NOT NULL,
[Rate] TINYINT NOT NULL,
CHECK ([Rate] BETWEEN 1 AND 10),
[RoomStatus] VARCHAR(10) FOREIGN KEY ([RoomStatus]) REFERENCES [RoomStatus]([RoomStatus]) NOT NULL,
[Notes] NVARCHAR(MAX)
)

CREATE TABLE [Payments](
[Id] INT PRIMARY KEY NOT NULL,
[EmployeeId] INT FOREIGN KEY ([EmployeeId]) REFERENCES [Employees]([Id]) NOT NULL,
[PaymentDate] DATE NOT NULL,
[AccountNumber] INT FOREIGN KEY ([AccountNumber]) REFERENCES [Customers]([AccountNumber]) NOT NULL,
[FirstDateOccupied] DATE NOT NULL,
[LastDateOccupied] DATE NOT NULL,
[TotalDays] INT NOT NULL,
[AmountCharged] INT NOT NULL,
[TaxRate] TINYINT NOT NULL,
[TaxAmount] INT NOT NULL,
[PaymentTotal] DECIMAL(7,2) NOT NULL,
[Notes] NVARCHAR(MAX)
)

CREATE TABLE [Occupancies](
[Id] INT PRIMARY KEY NOT NULL,
[EmployeeId] INT FOREIGN KEY ([EmployeeId]) REFERENCES [Employees]([Id]) NOT NULL,
[DateOccupied] DATE NOT NULL,
[AccountNumber] INT FOREIGN KEY ([AccountNumber]) REFERENCES [Customers]([AccountNumber]) NOT NULL,
[RoomNumber] INT FOREIGN KEY ([RoomNumber]) REFERENCES [Rooms]([RoomNumber]) NOT NULL,
[RateApplied] TINYINT NOT NULL,
[PhoneCharge] INT NOT NULL,
[Notes] NVARCHAR(MAX)
)

INSERT INTO [Employees]
	VALUES
	(1,'Gosho','Мениджър',NULL)
	,(2,'Todor','Boss', 'Много бачка')
	,(3,'Azis','Сен-Тропе','Други на Малдивите')

INSERT INTO [Customers]
	VALUES
	(42326,'Riko','Band',0981273423,'Rijko',0010,NULL)
	,(32225,'Rick','Sheen',0774213543,'Sheri', 9999, 'Humiliating Walkers')
	,(22114,'Xavier','Xavier',0753123543,'Xavi', 1121,NULL)

INSERT INTO [RoomStatus]
	VALUES
	('Free',NULL)
	,('Taken',NULL)
	,('Reserved','Best room yet')

INSERT INTO [RoomTypes]
	VALUES
	('Single',NULL)
	,('Double',NULL)
	,('Quad',NULL)

INSERT INTO [BedTypes]
	VALUES
	('Small',NULL)
	,('Medium',NULL)
	,('Big','Not so big')

INSERT INTO [Rooms]
	VALUES
	(144,'Double','Medium',8,'Free',NULL)
	,(251,'Quad','Big',9,'Taken',NULL)
	,(444,'Single','Small',6,'Reserved',NULL)

INSERT INTO [Payments]
	VALUES
	(1,1,'2022-05-21',42326,'2022-05-15','2022-05-21',6,500,20,100, 600,NULL)
	,(2,2,'2022-04-10',32225,'2022-04-1','2022-04-10',9,700,20,140,840,NULL)
	,(3,3,'2022-03-15',22114,'2022-03-1','2022-03-15',14,1000,20,200,1200,NULL)

INSERT INTO [Occupancies]
	VALUES
	(1,1,'2022-05-21',42326,144,20,50,NULL)
	,(2,2,'2022-04-10',32225,251,20,100,NULL)
	,(3,3,'2022-03-15',22114,444,20,500,NULL)