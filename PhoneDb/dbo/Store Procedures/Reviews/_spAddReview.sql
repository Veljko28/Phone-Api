CREATE PROCEDURE [dbo].[_spAddReview]
	@Id NVARCHAR(50),
	@Rating int,
	@BuyerId NVARCHAR(50),
	@SellerId NVARCHAR(50),
	@PhoneId NVARCHAR(50),
	@DateCreated DATETIME2(7),
	@Message NVARCHAR(256)
AS
begin
	INSERT INTO [dbo].[UserReviews] (Id, Rating, BuyerId, SellerId,PhoneId, DateCreated, [Message]) VALUES 
	(@Id, @Rating, @BuyerId, @SellerId, @PhoneId, @DateCreated, @Message);
end