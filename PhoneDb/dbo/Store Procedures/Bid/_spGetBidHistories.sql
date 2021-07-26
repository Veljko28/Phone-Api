CREATE PROCEDURE [dbo].[_spGetBidHistories]
	@Id NVARCHAR(50)
AS
begin
	SELECT * FROM [dbo].[BidHistories] WHERE Bid_Id = @Id;
end