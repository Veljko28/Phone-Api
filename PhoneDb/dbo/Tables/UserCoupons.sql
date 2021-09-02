﻿CREATE TABLE [dbo].[UserCoupons]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[UserId] NVARCHAR(50) NOT NULL,
	[Coupon] NVARCHAR(15) NOT NULL,
	[Amount] NVARCHAR(5) NOT NULL,
	[Valid] BIT NOT NULL DEFAULT 1
)
