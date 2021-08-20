CREATE PROCEDURE [dbo].[_spChangePhoneStatus]
	@Id NVARCHAR(50),
	@Status NVARCHAR(50)
AS
begin
	UPDATE [dbo].[Phones] SET [Status] = @Status WHERE Id = @Id;
end