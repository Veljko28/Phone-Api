CREATE PROCEDURE [dbo].[_spLoginUser]
	@Email_UserName NVARCHAR(150)
AS
begin
	SELECT * FROM [dbo].[Users] WHERE  ([dbo].[Users].[Email] = @Email_UserName OR [dbo].[Users].[UserName] = @Email_UserName);
end