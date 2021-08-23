CREATE PROCEDURE [dbo].[_spGetUserByEmail]
	@Email NVARCHAR(150)
AS
begin 
	SELECT * FROM [dbo].[Users] WHERE Email = @Email;
end