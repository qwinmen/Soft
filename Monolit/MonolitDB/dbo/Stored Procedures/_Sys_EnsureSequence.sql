CREATE PROCEDURE [dbo].[_Sys_EnsureSequence]
(	@schemaName		SYSNAME = 'dbo'
,	@sequenceName	SYSNAME
,	@startsFrom		BIGINT = 1
,	@increment		BIGINT = 1
)
AS
BEGIN
	SET NOCOUNT ON

	UPDATE	[dbo].[_Sys_Sequences]
		SET	[Increment] = @increment
	WHERE	[SchemaName] = @schemaName
		AND	[Name] = @sequenceName

	IF @@ROWCOUNT = 0
		INSERT
		INTO	[dbo].[_Sys_Sequences]
				([SchemaName], [Name], [Increment], [LastValue])
		VALUES	(@schemaName, @sequenceName, @increment, @startsFrom - @increment)

	RETURN 0
END
