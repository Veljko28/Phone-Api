CREATE PROCEDURE [dbo].[_spAddLoyalityPoints]
	@Id NVARCHAR(50)
AS
begin
	UPDATE [dbo].[Users] SET LoyalityPoints = (LoyalityPoints + 15) WHERE Id = @Id;
end