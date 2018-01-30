CREATE TABLE [dbo].[Objects]
(
	[id]	INT					NOT NULL PRIMARY KEY
,	[UID]	uniqueidentifier	NOT NULL default(newid())
,	[Name]	varchar(50)			NOT NULL
,	[isDeleted]	bit				NOT NULL default(0)
)
