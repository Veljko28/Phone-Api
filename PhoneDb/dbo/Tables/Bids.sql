﻿CREATE TABLE [dbo].[Bids]
(
	[Id] NVARCHAR(50) NOT NULL,
	[Name] NVARCHAR(100) NOT NULL,
	[Image] NVARCHAR(256) NOT NULL, 
	[Description] NVARCHAR(256) NOT NULL,
	[Price] MONEY NOT NULL,
	[Brand] NVARCHAR(25) NOT NULL,
	[Category] NVARCHAR(50) NOT NULL,
	[Seller] NVARCHAR(50) NOT NULL,
	[TimeCreated] DATETIME2(7) NOT NULL DEFAULT getutcdate(),
	[TimeEnds] DATETIME2(7) NOT NULL
)
