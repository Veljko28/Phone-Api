CREATE PROCEDURE [dbo].[_spRegisterUser]
	@Email NVARCHAR(150),
	@UserName NVARCHAR(150),
	@Password NVARCHAR(150)
AS
begin
	INSERT INTO [dbo].[Users] (Email, UserName, [Password]) VALUES (@Email, @UserName, @Password);
end