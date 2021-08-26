CREATE PROCEDURE [dbo].[_spGetBidHisotriesByUserName]
	@UserName NVARCHAR(150),
	@Page int
AS
begin
	 SELECT * FROM (
            SELECT ROW_NUMBER() OVER(ORDER BY Id) AS RoNum
                  , *
            FROM [dbo].[BidHistories]
    ) AS tbl 
    WHERE (@Page-1)*8 < RoNum AND [tbl].[UserName] = @UserName
    ORDER BY tbl.Id
end