USE [TranslatorApp]
GO

INSERT INTO [dbo].[Roles]
           ([Name])
     VALUES
           ('Admin'),
		   ('User')
GO

USE [TranslatorApp]
GO

INSERT INTO [dbo].[Users]
           ([Email]
           ,[Password]
           ,[RoleID])
     VALUES
           ('admin@gmail.com', 'admin', 1)
GO

select * from [dbo].[Roles]
select * from [dbo].[Users]
