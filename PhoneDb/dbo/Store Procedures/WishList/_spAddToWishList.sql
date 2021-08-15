CREATE PROCEDURE [dbo].[_spAddToWishList]
	@Id NVARCHAR(50),
	@UserId NVARCHAR(50),
	@PhoneId NVARCHAR(50),
	@Type NVARCHAR(5)
AS
begin
	INSERT INTO [dbo].[WishLists] (Id, UserId, PhoneId, [Type]) VALUES (@Id, @UserId, @PhoneId, @Type);
end