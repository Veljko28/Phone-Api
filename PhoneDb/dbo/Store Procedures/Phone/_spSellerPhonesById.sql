CREATE PROCEDURE [dbo].[_spSellerPhonesById]
	@Id NVARCHAR(50)
AS
begin
	SELECT * FROM [dbo].[Phones] WHERE Seller = @Id;
end