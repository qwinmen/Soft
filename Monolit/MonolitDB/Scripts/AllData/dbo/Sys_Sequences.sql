-- Merge data into [dbo].[_Sys_Sequences] table script.

PRINT N'Merging into [dbo].[_Sys_Sequences]...'

DECLARE @_Sys_Sequences TABLE
(	[ID] uniqueidentifier NOT NULL
,	[SchemaName] nvarchar(128) NOT NULL
,	[Name] nvarchar(128) NOT NULL
,	[Increment] bigint NOT NULL
,	[LastValue] bigint NOT NULL
)

SET NOCOUNT ON;


INSERT
INTO	@_Sys_Sequences
	(	[ID]
	,	[SchemaName]
	,	[Name]
	,	[Increment]
	,	[LastValue]
	)
VALUES	('81549e31-98d3-4a22-a3ce-3b4599395f06', N'dbo', N'ObjectID', 1, 1)

	;

SET NOCOUNT OFF;


MERGE
INTO	[dbo].[_Sys_Sequences] AS [target]
USING	@_Sys_Sequences AS [source]
	ON	[target].[ID] = [source].[ID]
WHEN MATCHED THEN
	UPDATE
		SET	[target].[SchemaName] = [source].[SchemaName]
		,	[target].[Name] = [source].[Name]
		,	[target].[Increment] = [source].[Increment]
		,	[target].[LastValue] = [source].[LastValue]
WHEN NOT MATCHED BY TARGET THEN
	INSERT
	(	[ID]
	,	[SchemaName]
	,	[Name]
	,	[Increment]
	,	[LastValue]
	)
	VALUES
	(	[source].[ID]
	,	[source].[SchemaName]
	,	[source].[Name]
	,	[source].[Increment]
	,	[source].[LastValue]
	)
	;

GO
