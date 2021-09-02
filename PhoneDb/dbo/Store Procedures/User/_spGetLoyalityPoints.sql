CREATE PROCEDURE [dbo].[_spGetLoyalityPoints]
	@Id NVARCHAR(50)
AS
begin
	SELECT LoyalityPoints FROM [dbo].[Users] WHERE Id = @Id;
end