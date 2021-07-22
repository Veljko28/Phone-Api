CREATE PROCEDURE [dbo].[_spGetPurchasedPhones]
	@PurchaseId NVARCHAR(50)
AS
begin
	SELECT PhoneId FROM [dbo].[PhonePurchases] WHERE PurchaseId = @PurchaseId;
end