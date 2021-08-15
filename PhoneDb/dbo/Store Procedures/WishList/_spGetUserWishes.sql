CREATE PROCEDURE [dbo].[_spGetUserWishes]
	@UserId NVARCHAR(50),
	@Type NVARCHAR(5)
AS
begin
	SELECT PhoneId FROM [dbo].[WishLists] WHERE UserId = @UserId AND [Type] = @Type;
end