CREATE PROCEDURE [dbo].[_spCheckInUseUserName]
	@UserName NVARCHAR(150)
AS
begin
	SELECT UserName FROM [dbo].[Users] WHERE UserName = @UserName;
end