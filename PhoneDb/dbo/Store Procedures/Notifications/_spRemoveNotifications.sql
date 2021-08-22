CREATE PROCEDURE [dbo].[_spRemoveNotifications]
	@Type NVARCHAR(10),
	@Name NVARCHAR(100),
	@UserId NVARCHAR(50),
	@Message NVARCHAR(MAX)
AS
begin
	DELETE FROM [dbo].[Notifications] WHERE [Type] = @Type AND [Name] = @Name AND UserId = @UserId AND @Message = @Message;
end

