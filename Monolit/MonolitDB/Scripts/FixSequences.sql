DECLARE @_Sys_Sequences TABLE
(	[SchemaName] nvarchar(128) NOT NULL
,	[Name] nvarchar(128) NOT NULL
,	[LastValue] bigint NOT NULL
)

SET NOCOUNT ON;

INSERT
INTO	@_Sys_Sequences
	(	[LastValue]
	,	[SchemaName]
	,	[Name]
	)

select	max(v.LastValue) as LastValue
	,	v.SchemaName
	,	v.Name
from	(
select	LastValue
	,	SchemaName
	,	Name
from	dbo._Sys_Sequences

---------------------------------------------------------------------------

-- [dbo].[Objects]
union	all
select	max(Revision) as LastValue
	,	'dbo' as SchemaName
	,	'Revision' as Name
from	[dbo].[Objects]

union	all
select	max(ID) as LastValue
	,	'dbo' as SchemaName
	,	'ObjectID' as Name
from	[dbo].[Objects]

--------------------------------------------------------------------------------
		) as v
where	v.LastValue is not null
group
	by	v.SchemaName
	,	v.Name

SET NOCOUNT OFF;

--SELECT	*
--FROM	@_Sys_Sequences

MERGE
INTO	[dbo].[_Sys_Sequences] AS [target]
USING	@_Sys_Sequences AS [source]
	ON	[target].[SchemaName] = [source].[SchemaName]
	AND	[target].[Name] = [source].[Name]
WHEN MATCHED THEN
	UPDATE
		SET	[target].[SchemaName] = [source].[SchemaName]
		,	[target].[Name] = [source].[Name]
		,	[target].[LastValue] = [source].[LastValue]
	;
