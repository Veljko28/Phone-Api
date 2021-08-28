CREATE PROCEDURE [dbo].[_spGetNumberOfUserPhones]
	@UserId NVARCHAR(50)
AS
begin
	SELECT COUNT(*) FROM [dbo].[Phones] WHERE Seller = @UserId AND [Status] = 0;
end