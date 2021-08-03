﻿CREATE PROCEDURE [dbo].[_spAddBid]
	@Id NVARCHAR(50),
	@Name NVARCHAR(100),
	@Image NVARCHAR(256),
	@Description NVARCHAR(256),
	@Price MONEY,
	@Brand NVARCHAR(25),
	@Category NVARCHAR(50),
	@Seller NVARCHAR(50),
	@TimeCreated DATETIME2(7),
	@TimeEnds DATETIME2(7)
AS
begin
	INSERT INTO [dbo].[Bids] (Id, [Name],[Image], [Description], Price, Brand, Category, Seller, TimeCreated, TimeEnds )
	VALUES (@Id, @Name, @Image, @Description, @Price, @Brand, @Category, @Seller, @TimeCreated, @TimeEnds );
end