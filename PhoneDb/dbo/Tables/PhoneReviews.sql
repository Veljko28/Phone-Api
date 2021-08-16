﻿CREATE TABLE [dbo].[PhoneReviews]
(
	[Id] NVARCHAR(50) NOT NULL PRIMARY KEY,
	[Rating] INT NOT NULL,
	[Message] NVARCHAR(256) NOT NULL,
	[UserId] NVARCHAR(50) NOT NULL,
	[PhoneId] NVARCHAR(50) NOT NULL,
	[DateCreated] DATETIME2(7) NOT NULL DEFAULT getutcdate()
)