CREATE PROCEDURE [dbo].[_spUpdateUserRating]
	@UserId NVARCHAR(50),
	@Rating DECIMAL(3,2)
AS
begin
	UPDATE [dbo].[Users] SET Rating = @Rating WHERE Id = @UserId;
end