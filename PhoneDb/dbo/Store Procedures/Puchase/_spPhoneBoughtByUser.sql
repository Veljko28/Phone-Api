CREATE PROCEDURE [dbo].[_spPhoneBoughtByUser]
	@UserId NVARCHAR(50),
	@PhoneId NVARCHAR(50)
AS
begin
	SELECT * FROM [dbo].[PhonePurchases] WHERE BuyerId = @UserId AND PhoneId = @PhoneId;
end