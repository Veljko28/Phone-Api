CREATE PROCEDURE [dbo].[_spAddToWishList]
	@Id NVARCHAR(50),
	@UserId NVARCHAR(50),
	@PhoneId NVARCHAR(50)
AS
begin
	INSERT INTO [dbo].[WishLists] (Id, UserId, PhoneId) VALUES (@Id, @UserId, @PhoneId);
end