CREATE DATABASE [CarRental]
USE [CarRental]

CREATE TABLE [Categories](
[Id] INT PRIMARY KEY NOT NULL,
[CategoryName] NVARCHAR(30) NOT NULL,
[DailyRate] INT NOT NULL,
[WeeklyRate] INT NOT NULL,
[MonthlyRate] INT NOT NULL,
[WeekendRate] INT NOT NULL,
)

CREATE TABLE [Cars](
[Id] INT PRIMARY KEY NOT NULL,
[PlateNumber] VARCHAR(10) NOT NULL,
[Manufacturer] VARCHAR(30) NOT NULL,
[Model] VARCHAR(30) NOT NULL,
[CarYear] DATE NOT NULL,
[CategoryId] INT FOREIGN KEY ([CategoryId]) REFERENCES [Categories]([Id]) NOT NULL,
[Doors] INT NOT NULL,
CHECK ([Doors] >= 2 OR [Doors] <= 5),
[Picture] VARBINARY(MAX),
[Condition] VARCHAR(10) NOT NULL,
CHECK ([Condition] = 'Good' OR [Condition] = 'Bad'),
[Available] CHAR(1) NOT NULL,
CHECK ([Available] = '0' OR [Available] = '1')
)

CREATE TABLE [Employees](
[Id] INT PRIMARY KEY NOT NULL,
[FirstName] NVARCHAR(50) NOT NULL,
[LastName] NVARCHAR(50) NOT NULL,
[Title] VARCHAR(30) NOT NULL,
[Notes] NVARCHAR(MAX)
)

CREATE TABLE [Customers](
[Id] INT PRIMARY KEY NOT NULL,
[DriverLicenseNumber] INT NOT NULL,
[FullName] NVARCHAR(50) NOT NULL,
[Adress] NVARCHAR(50) NOT NULL,
[City] NVARCHAR(50) NOT NULL,
[ZIPCode] TINYINT NOT NULL,
[Notes] NVARCHAR(MAX)
)

CREATE TABLE [RentalOrders](
[Id] INT PRIMARY KEY NOT NULL,
[EmployeeId] INT FOREIGN KEY ([EmployeeId]) REFERENCES [Employees]([Id]) NOT NULL,
[CustomerId] INT FOREIGN KEY ([CustomerId]) REFERENCES [Customers]([Id]) NOT NULL,
[CarId] INT FOREIGN KEY ([CarId]) REFERENCES [Cars]([Id]) NOT NULL,
[TankLevel] INT NOT NULL,
[KilometrageStart] INT NOT NULL,
[KilometrageEnd] INT NOT NULL,
[TotalKilometrage] INT NOT NULL,
[StartDate] DATE NOT NULL,
[EndDate] DATE NOT NULL,
[TotalDays] INT NOT NULL,
[RateApplied] INT NOT NULL,
[TaxRate] INT NOT NULL,
[OrderStatus] VARCHAR(10) NOT NULL,
CHECK ([OrderStatus] = 'Ongoing' OR [OrderStatus] = 'Finished'),
[Notes] NVARCHAR(MAX)
)

INSERT INTO [Categories]([Id],[CategoryName],[DailyRate],[WeeklyRate],[MonthlyRate],[WeekendRate])
	VALUES
	(1,'SportsCar', 20, 15, 14, 18),
	(2, 'Car', 11, 8, 6, 9),
	(3, 'DriftCar', 30,24,22,27)

INSERT INTO [Cars]([Id],[PlateNumber],[Manufacturer],[Model],[CarYear],[CategoryId],[Doors],[Condition],[Available])
	VALUES
	(1, 'CB1232BC', 'VW','GolfMK5', '2007-6-20', 2, 4, 'Good', 1),
	(2,'H9999CC', 'BMW', '530d', '2001-3-15', 3, 4, 'Good',1),
	(3,'R3294HC', 'Chevrolet', 'Camaro', '2010-8-16', 1, 2, 'Good', 0)

INSERT INTO [Employees]([Id],[FirstName],[LastName],[Title])
	VALUES
	(1,'Pesho','Goshkov','Worker'),
	(2,'Tosho','Toshkov','Manager'),
	(3,'Rado','Peshov','Boss')

INSERT INTO [Customers]([Id],[DriverLicenseNumber],[FullName],[Adress],[City],[ZIPCode])
	VALUES
	(1,4334,'Vasko','ul.Klokotnica','Around',220),
	(2,3322,'Goshko','ul.Hristo Botev','Varna',210),
	(3,1111,'Jorko','ul.Vasil Levski','Sofia',100)

INSERT INTO [RentalOrders]([Id],[EmployeeId],[CustomerId],
			[CarId],[TankLevel],[KilometrageStart],[KilometrageEnd],[TotalKilometrage],
			[StartDate],[EndDate],[TotalDays],[RateApplied],[TaxRate],[OrderStatus])
      VALUES
	  (1,1,1,1,100,10000,15000,25000,'2022-1-10','2022-2-10',30,20,20,'Finished'),
	  (2,2,2,2,200,25000,30000,55000,'2021-12-10','2022-3-10',90,10,5,'Finished'),
	  (3,3,3,3,300,40000,50000,90000,'2021-1-1','2022-1-1',365,2,2,'Finished')