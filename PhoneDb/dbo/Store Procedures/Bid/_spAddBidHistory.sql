CREATE PROCEDURE [dbo].[_spAddBidHistory]
	@Id NVARCHAR(50),
	@Bid_Id NVARCHAR(50),
	@UserName NVARCHAR(150),
	@Amount MONEY
AS
begin
	INSERT INTO [dbo].[BidHistories] (Id, Bid_Id, UserName, Amount) VALUES (@Id, @Bid_Id, @UserName, @Amount);
end