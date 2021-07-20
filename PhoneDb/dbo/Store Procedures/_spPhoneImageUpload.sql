CREATE PROCEDURE [dbo].[_spPhoneImageUpload]
	@Id NVARCHAR(50),
	@Image NVARCHAR(256)
AS
begin
  UPDATE [dbo].[Phones] SET [Image] = @Image WHERE Id = @Id;
end