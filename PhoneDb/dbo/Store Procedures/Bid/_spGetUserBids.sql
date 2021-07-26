CREATE PROCEDURE [dbo].[_spGetUserBids]
	@Id NVARCHAR(50)
AS
begin
	SELECT * FROM [dbo].[Bids] WHERE Seller = @Id;
end