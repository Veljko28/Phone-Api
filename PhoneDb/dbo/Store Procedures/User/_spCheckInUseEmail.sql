CREATE PROCEDURE [dbo].[_spCheckInUseEmail]
	@Email NVARCHAR(150)
AS
begin
	SELECT Email FROM [dbo].[Users] WHERE Email = @Email;
end