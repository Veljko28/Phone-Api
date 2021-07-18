CREATE PROCEDURE [dbo].[_spLoginUser]
	@Email_UserName NVARCHAR(150),
	@Password NVARCHAR(150)
AS
begin
	SELECT * FROM [dbo].[Users] WHERE (Email = @Email_UserName OR UserName = @Email_UserName) AND [Password] = @Password;
end