CREATE TABLE [dbo].[Users]
(
	[Id] NVARCHAR(50) NOT NULL PRIMARY KEY,
	[Email] NVARCHAR(150) NOT NULL,
	[UserName] NVARCHAR(150) NOT NULL,
	[Password] NVARCHAR(256) NOT NULL, 
	[Description] NVARCHAR(256) NULL,
	[Rating] INT NULL,
	[PhoneNumber] NVARCHAR(100) NULL,
	[Phones_sold] int NOT NULL DEFAULT 0,
	[EmailConfirmed] bit NOT NULL DEFAULT 0,
	[LoyalityPoints] int NOT NULL DEFAULT 0
)
