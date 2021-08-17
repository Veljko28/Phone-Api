CREATE PROCEDURE [dbo].[_spRemoveWish]
	@UserId NVARCHAR(50),
	@PhoneId NVARCHAR(50)
AS
begin
	DELETE FROM [dbo].[WishLists] WHERE UserId = @UserId AND PhoneId = @PhoneId;
end