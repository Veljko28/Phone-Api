﻿CREATE TABLE [dbo].[BidHistories]
(
	[Id] NVARCHAR(50) NOT NULL PRIMARY KEY,
	[Bid_Id] NVARCHAR(50) NOT NULL,
	[UserName] NVARCHAR(150) NOT NULL,
	[Amount] MONEY NOT NULL
)
