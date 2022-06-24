CREATE TABLE [Students](
[StudentID] INT PRIMARY KEY IDENTITY NOT NULL,
[Name] NVARCHAR(50) NOT NULL
)

INSERT INTO [Students]
	VALUES
	('Mila')
	,('Toni')
	,('Ron')

CREATE TABLE [Exams](
[ExamID] INT PRIMARY KEY IDENTITY(101,1) NOT NULL,
[Name] NVARCHAR(50) NOT NULL
)

INSERT INTO [Exams]
	VALUES
	('SpringMVC')
	,('Neo4j')
	,('Oracle 11g')


CREATE TABLE [StudentsExams](
[StudentID] INT NOT NULL,
[ExamID] INT NOT NULL,
CONSTRAINT CPK_Students_Exams_ID PRIMARY KEY ([StudentID],[ExamID]),
CONSTRAINT FK_StudentID FOREIGN KEY ([StudentID]) REFERENCES [Students]([StudentID]),
CONSTRAINT FK_ExamID FOREIGN KEY ([ExamID]) REFERENCES [Exams]([ExamID])
)

INSERT INTO [StudentsExams]
	VALUES
	(1,101)
	,(1,102)
	,(2,101)
	,(3,103)
	,(2,102)
	,(2,103)

	