﻿CREATE TABLE [dbo].[ContactSupport]
(
	[Id] NVARCHAR(50) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(100) NOT NULL, 
    [Email] NVARCHAR(150) NOT NULL, 
    [Subject] NVARCHAR(100) NOT NULL, 
    [Message] NVARCHAR(MAX) NOT NULL,

)
