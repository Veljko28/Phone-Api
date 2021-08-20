CREATE PROCEDURE [dbo].[_spGetUserNotifications]
	@Id NVARCHAR(50)
AS
begin
	SELECT * FROM [dbo].[Notifications] WHERE UserId = @Id;
end