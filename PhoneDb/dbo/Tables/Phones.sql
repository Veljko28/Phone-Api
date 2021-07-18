CREATE TABLE [dbo].[Phones]
(
	[Id] NVARCHAR(50) NOT NULL PRIMARY KEY, 
    [Image] NVARCHAR(150) NULL, 
    [Name] NVARCHAR(100) NOT NULL,
	[Description] NVARCHAR(256) NULL, 
    [DateCreated] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [Seller] NVARCHAR(50) NOT NULL, 
    [Category] NVARCHAR(50) NOT NULL, 
    [Brand] NVARCHAR(25) NOT NULL,

)
