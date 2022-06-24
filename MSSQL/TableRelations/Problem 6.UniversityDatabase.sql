CREATE DATABASE [University]
USE [University]

CREATE TABLE [Majors](
[MajorID] INT PRIMARY KEY NOT NULL,
[Name] VARCHAR(50) NOT NULL
)

CREATE TABLE [Students](
[StudentID] INT PRIMARY KEY NOT NULL,
[StudentNumber] INT NOT NULL,
[StudentName] VARCHAR(50) NOT NULL,
[MajorID] INT FOREIGN KEY ([MajorID]) REFERENCES [Majors]([MajorID])
)

CREATE TABLE [Payments](
[PaymentID] INT PRIMARY KEY NOT NULL,
[PaymentDate] DATE NOT NULL,
[PaymentAmount] DECIMAL(9,2) NOT NULL,
[StudentID] INT FOREIGN KEY ([StudentID]) REFERENCES [Students]([StudentID])
)

CREATE TABLE [Subjects](
[SubjectID] INT PRIMARY KEY NOT NULL,
[SubjectName] VARCHAR(50) NOT NULL
)

CREATE TABLE [Agenda](
[StudentID] INT NOT NULL, 
[SubjectID] INT NOT NULL,
CONSTRAINT PK_StudentID_SubjectID_Agenda PRIMARY KEY ([StudentID],[SubjectID]),
CONSTRAINT FK_StudentID_Agenda FOREIGN KEY ([StudentID]) REFERENCES [Students]([StudentID]),
CONSTRAINT FK_SubjectID_Agenda FOREIGN KEY ([SubjectID]) REFERENCES [Subjects]([SubjectID])
)