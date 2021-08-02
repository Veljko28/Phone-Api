CREATE PROCEDURE [dbo].[_spGetPhonePage]
	@Page int
AS
begin
	 SELECT * FROM (
            SELECT ROW_NUMBER() OVER(ORDER BY Id) AS RoNum
                  , *
            FROM [dbo].[Phones]
    ) AS tbl 
    WHERE (@Page-1)*15 < RoNum
    ORDER BY tbl.Id
end
