CREATE TABLE [dbo].[PhonePurchases]
(
	[Id] int PRIMARY KEY IDENTITY,
	[PhoneId] NVARCHAR(50) NOT NULL,
	[SellerId] NVARCHAR(50) NOT NULL,
	[BuyerId] NVARCHAR(50) NOT NULL,
	[PurchaseDate] DATETIME2(7) NOT NULL DEFAULT getutcdate()
)
