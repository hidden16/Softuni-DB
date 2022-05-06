CREATE DATABASE [TableRelations]

CREATE TABLE [Persons](
[PersonID] INT NOT NULL,
[FirstName] NVARCHAR(50) NOT NULL,
[Salary] DECIMAL(9,2) NOT NULL,
[PassportID] INT NOT NULL
)

CREATE TABLE [Passports](
[PassportID] INT PRIMARY KEY NOT NULL,
[PassportNumber] VARCHAR(60) NOT NULL
)

ALTER TABLE [Persons]
ADD PRIMARY KEY (PersonID)

ALTER TABLE [Persons]
ADD FOREIGN KEY (PassportID) REFERENCES [Passports]([PassportID])