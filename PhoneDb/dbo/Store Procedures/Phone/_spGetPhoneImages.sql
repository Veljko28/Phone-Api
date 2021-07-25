CREATE PROCEDURE [dbo].[_spGetPhoneImages]
	@Id NVARCHAR(50)
AS
begin
	SELECT ImagePath from [dbo].[PhoneImages] WHERE PhoneId = @Id;
end