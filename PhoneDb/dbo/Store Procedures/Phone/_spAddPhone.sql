CREATE PROCEDURE [dbo].[_spAddPhone]
	@Id NVARCHAR(50),
	@Seller NVARCHAR(50),
	@Price MONEY,
	@Description NVARCHAR(256),
	@Name NVARCHAR(100),
	@Category NVARCHAR(50),
	@Brand NVARCHAR(25)
AS
begin
	INSERT INTO [dbo].[Phones] (Id, Seller, Price, [Description], [Name], Category, Brand) VALUES (@Id, @Seller, @Price, @Description, @Name, @Category, @Brand);
end