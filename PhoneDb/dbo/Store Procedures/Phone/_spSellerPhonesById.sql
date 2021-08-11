CREATE PROCEDURE [dbo].[_spSellerPhonesById]
	@Id NVARCHAR(50),
    @Page int
AS
begin
	SELECT TOP 8 * FROM (
            SELECT ROW_NUMBER() OVER(ORDER BY Id) AS RoNum
                  , *
            FROM [dbo].[Phones] WHERE Seller = @Id
    ) AS tbl 
    WHERE (@Page-1)*8 < RoNum
    ORDER BY tbl.Id
end