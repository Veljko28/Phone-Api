CREATE PROCEDURE [dbo].[_spGetUserIdByName]
	@UserName NVARCHAR(150)
AS
begin
	SELECT Id FROM [dbo].[Users] WHERE UserName = @UserName;
end