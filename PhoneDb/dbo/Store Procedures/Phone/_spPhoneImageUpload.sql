CREATE PROCEDURE [dbo].[_spPhoneImageUpload]
	@Id NVARCHAR(50),
	@Image NVARCHAR(256)
AS
begin
  INSERT INTO  [dbo].[PhoneImages] (PhoneId, ImagePath) VALUES (@Id, @Image);
end