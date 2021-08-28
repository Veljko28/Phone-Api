CREATE PROCEDURE [dbo].[_spAddPurchase]
	@BuyerId NVARCHAR(50),
	@SellerId NVARCHAR(50),
	@PhoneId NVARCHAR(50)
AS
begin
	INSERT INTO [dbo].[PhonePurchases] ( BuyerId, SellerId, PhoneId) VALUES ( @BuyerId, @SellerId, @PhoneId );
end