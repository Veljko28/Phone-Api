CREATE PROCEDURE [dbo].[_spAddPurchase]
	@Id NVARCHAR(50),
	@UserId NVARCHAR(50),
	@Total MONEY,
	@PurchaseDate DATETIME2(7)
AS
begin
	INSERT INTO [dbo].[Purchases] (Id, UserId, Total, PurchaseDate) VALUES (@Id, @UserId, @Total, @PurchaseDate);
end