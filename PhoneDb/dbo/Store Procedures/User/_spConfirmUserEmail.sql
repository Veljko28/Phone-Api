CREATE PROCEDURE [dbo].[_spConfirmUserEmail]
	@Id NVARCHAR(50)
AS
begin
	UPDATE [dbo].[Users] SET EmailConfirmed = 1 WHERE Id = @Id;
end