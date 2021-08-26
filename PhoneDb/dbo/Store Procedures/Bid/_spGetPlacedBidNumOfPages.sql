CREATE PROCEDURE [dbo].[_spGetPlacedBidNumOfPages]
	@UserName NVARCHAR(150)
AS
begin
	SELECT COUNT(*) FROM [dbo].[BidHistories] WHERE UserName = @UserName;
end	