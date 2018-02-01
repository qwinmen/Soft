CREATE FUNCTION [dbo].[_Sys_GetSequenceNextValue]
(	@schemaName		SYSNAME
,	@sequenceName	SYSNAME
)
RETURNS BIGINT
AS
BEGIN
	DECLARE	@sql NVARCHAR(MAX)
	SELECT @schemaName = COALESCE(@schemaName, 'dbo')
	SET	@sql = N'
UPDATE	[dbo].[_Sys_Sequences] WITH (ROWLOCK, HOLDLOCK)
	SET	[LastValue]=[LastValue]+[Increment]
WHERE	[SchemaName] = ''' + @schemaName + '''
	AND	[Name] = ''' + @sequenceName + '''
	;

SELECT	[LastValue]
FROM	[dbo].[_Sys_Sequences]
WHERE	[SchemaName] = ''' + @schemaName + '''
	AND	[Name]=''' + @sequenceName + '''
	;
';
	RETURN [$(DemSystemDB)].dbo.GetBigIntInAutonomousTransaction(DB_NAME(), @sql);
END
