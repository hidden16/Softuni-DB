CREATE TABLE [People](
[Id] INT PRIMARY KEY IDENTITY(1,1),
[Name] NVARCHAR(200) NOT NULL,
[Picture] VARBINARY(MAX),
CHECK (DATALENGTH ([Picture]) <= 2000000),
[Height] DECIMAL(3,2),
[Weight] DECIMAL(3,2),
[Gender] char(1) NOT NULL,
CHECK ([Gender] = 'm' OR [Gender] = 'f'),
[Birthdate] DATE NOT NULL,
[Biography] NVARCHAR(MAX)
)

INSERT INTO [People]([Name], [Height], [Weight], [Gender], [Birthdate])
	VALUES
('Pesho', 1.85, 8.6, 'm', '2002-8-16'),
('Rado', 1.90, 9.2, 'm', '1999-12-3'),
('Radka', 1.56, 4.1, 'f', '2003-3-15'),
('Zuki', 1.65, 4.3,'f','2002-10-19'),
('Vesaka', 1.80, 3.5, 'm', '2001-7-12')