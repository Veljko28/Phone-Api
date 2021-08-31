CREATE PROCEDURE [dbo].[_spGetUserReviews]
	@SellerId NVARCHAR(50)
AS
begin
	SELECT * FROM [dbo].[UserReviews] WHERE SellerId = @SellerId;
end