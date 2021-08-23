CREATE PROCEDURE [dbo].[_spGetUserEmailById]
	@Id NVARCHAR(50)
AS
begin
	SELECT Email from [dbo].[Users] WHERE Id = @Id;
end	