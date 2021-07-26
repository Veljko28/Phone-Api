CREATE PROCEDURE [dbo].[_spRegisterUser]
	@Id NVARCHAR(50),
	@Email NVARCHAR(150),
	@UserName NVARCHAR(150),
	@Password NVARCHAR(150)
AS
begin
	INSERT INTO [dbo].[Users] (Id, Email, UserName, [Password]) VALUES (@Id, @Email, @UserName, @Password);
end