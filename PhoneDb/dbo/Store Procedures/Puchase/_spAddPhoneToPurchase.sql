CREATE PROCEDURE [dbo].[_spAddPhoneToPurchase]
	@PurchaseId NVARCHAR(50),
	@PhoneId NVARCHAR(50)
AS
begin
	INSERT INTO [dbo].[PhonePurchases] (PurchaseId, PhoneId) VALUES (@PurchaseId, @PhoneId);
end
