CREATE PROCEDURE [dbo].[_spAddPhone]
	@Id NVARCHAR(50),
	@Seller NVARCHAR(50),
	@Price MONEY,
	@Name NVARCHAR(100),
	@Image NVARCHAR(150),
	@Category NVARCHAR(50),
	@Brand NVARCHAR(25)
AS
begin
	INSERT INTO [dbo].[Phones] (Id, Seller, Price, [Name], [Image], Category, Brand) VALUES (@Id, @Seller, @Price, @Name, @Image, @Category, @Brand);
end