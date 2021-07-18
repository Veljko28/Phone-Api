CREATE PROCEDURE [dbo].[_spFindPhoneByID]
	@Id NVARCHAR(50)
AS
begin
	SELECT * FROM [dbo].[Phones] WHERE Id = @Id;
end	