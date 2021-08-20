CREATE PROCEDURE [dbo].[_spAddNotifications]
	@Id NVARCHAR(50),
	@Type NVARCHAR(10),
	@Name NVARCHAR(100),
	@UserId NVARCHAR(50),
	@Message NVARCHAR(MAX)
AS
begin
	INSERT INTO [dbo].[Notifications] (Id, [Type], [Name], UserId, [Message]) VALUES (@Id, @Type, @Name, @UserId, @Message);
end