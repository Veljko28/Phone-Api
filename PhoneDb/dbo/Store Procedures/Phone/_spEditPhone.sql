CREATE PROCEDURE [dbo].[_spEditPhone]
	@Id NVARCHAR(50),
	@Image NVARCHAR(256),
	@Name NVARCHAR(100),
	@Description NVARCHAR(256),
	@Price MONEY,
	@Seller NVARCHAR(50),
	@Category NVARCHAR(50),
	@DateCreated DATETIME2(7),
	@Brand NVARCHAR(25),
	@Status NVARCHAR(50)
AS
begin
	UPDATE [dbo].[Phones] SET [Image] = @Image, [Name] = @Name, [Description] = @Description, Price = @Price, Seller =  @Seller, Category = @Category,
	DateCreated = @DateCreated, Brand = @Brand, [Status] = @Status WHERE Id = @Id
end