CREATE PROCEDURE [dbo].[_spRemoveLoyalityPoints]
	@Id NVARCHAR(50),
	@Amount int
AS
begin
	UPDATE [dbo].[Users] SET LoyalityPoints = (LoyalityPoints - @Amount) WHERE Id = @Id;
end