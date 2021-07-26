CREATE PROCEDURE [dbo].[_spChangePassword]
	@Id NVARCHAR(50),
	@Password NVARCHAR(150)
AS
begin
	UPDATE [dbo].[Users] SET [Password] = @Password WHERE Id = @Id;
end
