CREATE PROCEDURE [dbo].[_spAddLoyalityPointsAmount]
	@Id NVARCHAR(50),
	@Amount int
AS
begin
	UPDATE [dbo].[Users] SET LoyalityPoints = (LoyalityPoints + @Amount) WHERE Id = @Id;
end