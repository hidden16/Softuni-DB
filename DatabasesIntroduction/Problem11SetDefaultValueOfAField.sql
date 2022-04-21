ALTER TABLE [Users]
	ADD CONSTRAINT DEF_Users_LastLoginTime_CurrTime 
	DEFAULT GETDATE()
	FOR [LastLoginTime]