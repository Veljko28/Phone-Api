CREATE PROCEDURE [dbo].[_spUpdateDisplayImage]
	@ImagePath NVARCHAR(256),
	@PhoneId NVARCHAR(50)
AS
begin
	UPDATE [dbo].[Phones] SET [Image] = @ImagePath WHERE Id = @PhoneId;
end	