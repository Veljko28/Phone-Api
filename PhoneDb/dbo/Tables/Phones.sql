﻿CREATE TABLE [dbo].[Phones]
(
	[Id] NVARCHAR(50) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(100) NOT NULL,
	[Image] NVARCHAR(256) NOT NULL, 
	[Description] NVARCHAR(256) NULL, 
    [DateCreated] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [Seller] NVARCHAR(50) NOT NULL, 
    [Category] NVARCHAR(50) NOT NULL, 
    [Brand] NVARCHAR(25) NOT NULL, 
    [Price] MONEY NOT NULL, 
    [Status] nvarchar(50) NULL,

)
