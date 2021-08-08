CREATE PROCEDURE [dbo].[_spUpdateBidPrice]
	@Id NVARCHAR(50),
	@Price MONEY
AS
begin
	UPDATE [dbo].[Bids] SET Price = @Price WHERE Id = @Id
end	