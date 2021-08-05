CREATE PROCEDURE [dbo].[_spDeleteBid]
	@Id nvarchar(50)
AS
begin
	DELETE FROM [dbo].[Bids] WHERE Id = @Id;
end