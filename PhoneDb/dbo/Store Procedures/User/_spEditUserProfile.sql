CREATE PROCEDURE [dbo].[_spEditUserProfile]
	@Id NVARCHAR(50),
	@UserName NVARCHAR(150),
	@Email NVARCHAR(150),
	@Description NVARCHAR(256),
	@PhoneNumber NVARCHAR(100)
AS
begin
	UPDATE [dbo].[Users] SET UserName = @UserName, Email = @Email, [Description] = @Description, PhoneNumber = @PhoneNumber WHERE Id = @Id
end