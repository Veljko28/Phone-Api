CREATE PROCEDURE [dbo].[_spAddPhone]
	@Id NVARCHAR(50),
	@Image NVARCHAR(256),
	@Seller NVARCHAR(50),
	@Price MONEY,
	@Description NVARCHAR(256),
	@Name NVARCHAR(100),
	@Category NVARCHAR(50),
	@Brand NVARCHAR(25),
	@Status int
AS
begin
	INSERT INTO [dbo].[Phones] (Id, [Image], Seller, Price, [Description], [Name], Category, Brand, [Status]) 
	VALUES (@Id, @Image, @Seller, @Price, @Description, @Name, @Category, @Brand, @Status);
end