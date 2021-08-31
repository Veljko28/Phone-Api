CREATE PROCEDURE [dbo].[_spFindUserReview]
	@BuyerId NVARCHAR(50),
	@PhoneId NVARCHAR(50)
AS
begin
	SELECT * FROM [dbo].[UserReviews] WHERE BuyerId = @BuyerId AND PhoneId = @PhoneId;
end