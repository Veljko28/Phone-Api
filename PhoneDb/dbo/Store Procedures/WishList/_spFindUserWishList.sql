CREATE PROCEDURE [dbo].[_spFindUserWishList]
	@UserId NVARCHAR(50),
	@PhoneId NVARCHAR(50),
	@Type NVARCHAR(5)
AS
begin
	SELECT * FROM [dbo].[WishLists] WHERE UserId = @UserId AND PhoneId = @PhoneId AND [Type] = @Type;
end