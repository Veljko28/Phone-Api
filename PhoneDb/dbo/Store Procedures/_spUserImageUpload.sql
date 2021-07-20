CREATE PROCEDURE [dbo].[_spUserImageUpload]
	@Id NVARCHAR(50),
	@Image NVARCHAR(256)
AS
begin
  UPDATE [dbo].[Users] SET [Image] = @Image WHERE Id = @Id;
end