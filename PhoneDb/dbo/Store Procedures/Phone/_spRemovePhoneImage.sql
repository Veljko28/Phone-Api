CREATE PROCEDURE [dbo].[_spRemovePhoneImage]
	@ImagePath NVARCHAR(256),
	@PhoneId NVARCHAR(50)
AS
begin
	DELETE FROM [dbo].[PhoneImages] WHERE ImagePath = @ImagePath AND PhoneId = @PhoneId;
end
