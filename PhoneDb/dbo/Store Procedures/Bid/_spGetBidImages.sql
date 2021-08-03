CREATE PROCEDURE [dbo].[_spGetBidImages]
	@Id NVARCHAR(50)
AS
begin
	SELECT ImagePath from [dbo].[BidImages] WHERE Bid_Id = @Id;
end