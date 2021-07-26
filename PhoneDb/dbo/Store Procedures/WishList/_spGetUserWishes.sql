CREATE PROCEDURE [dbo].[_spGetUserWishes]
	@UserId NVARCHAR(50)
AS
begin
	SELECT PhoneId FROM [dbo].[WishLists] WHERE UserId = @UserId;
end