CREATE PROCEDURE [dbo].[_spGetBidById]
	@Id NVARCHAR(50)
AS
begin
	SELECT * FROM [dbo].[Bids] WHERE Id = @Id;
end
