CREATE PROCEDURE [dbo].[_spRemoveNotifications]
	@Id NVARCHAR(50)
AS
begin
	DELETE FROM [dbo].[Notifications] WHERE Id = @Id;
end

