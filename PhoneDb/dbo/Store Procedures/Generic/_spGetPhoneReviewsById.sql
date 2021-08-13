CREATE PROCEDURE [dbo].[_spGetPhoneReviewsById]
	@Id NVARCHAR(50)
AS
begin
	SELECT * FROM [dbo].[PhoneReviews] WHERE PhoneId = @Id;
end