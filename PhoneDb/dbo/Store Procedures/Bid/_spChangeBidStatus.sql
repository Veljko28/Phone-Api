CREATE PROCEDURE [dbo].[_spChangeBidStatus]
	@Id NVARCHAR(50),
	@Status NVARCHAR(5)
AS
begin
	UPDATE [dbo].[Bids] SET [Status] = @Status WHERE Id = @Id;
end
