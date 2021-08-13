CREATE PROCEDURE [dbo].[_spAddReview]
	@Id NVARCHAR(50),
	@Message NVARCHAR(256),
	@Rating int,
	@UserId NVARCHAR(50),
	@PhoneId NVARCHAR(50),
	@DateCreated DATETIME2(7)
AS
begin
	INSERT INTO [dbo].[PhoneReviews] (Id, Rating, [Message], UserId, PhoneId, DateCreated) 
	VALUES (@Id, @Rating, @Message, @UserId, @PhoneId, @DateCreated);
end