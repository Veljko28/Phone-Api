CREATE PROCEDURE [dbo].[_spSearchBids]
	@Term NVARCHAR(256)
AS
begin
	SELECT * FROM [dbo].[Bids] WHERE [Name] LIKE @Term OR [Description] LIKE @Term OR Brand LIKE @Term;
end