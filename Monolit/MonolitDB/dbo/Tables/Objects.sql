CREATE TABLE [dbo].[Objects]
(
	[ID]					INT					NOT NULL	CONSTRAINT [DF_Objects_ID] DEFAULT ([dbo].[_Sys_GetSequenceNextValue]('dbo', 'ObjectID'))
,	[DocID]					AS (isnull(CONVERT([bigint],CONVERT([binary](4),(2201),(0))+CONVERT([binary](4),[ID],(0)),(0)),(0))) PERSISTED NOT NULL
,	[UID]					UNIQUEIDENTIFIER	NOT	NULL	CONSTRAINT [DF_Object_UID] DEFAULT (newid())
,	[IsDeleted]				BIT					NOT	NULL	CONSTRAINT [DF_Object_IsDeleted] DEFAULT ((0))
,	[Revision]				BIGINT				NOT	NULL	CONSTRAINT [DF_Object_Revision] DEFAULT ((0))
,	[Name]					varchar(50)			NOT NULL
,	CONSTRAINT [PK_Object] PRIMARY KEY NONCLUSTERED ([UID])
)
GO

CREATE TRIGGER [dbo].[TR_Object_UpdateRevision]
	ON	[dbo].[Objects]
	FOR		INSERT
		,	UPDATE
	NOT FOR REPLICATION
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE	@Count INT
		,	@Revision BIGINT

	DECLARE @Inserted TABLE
	(	[ID]					INT					NOT	NULL
	,	[UID]					UNIQUEIDENTIFIER	NOT	NULL
	,	[IsDeleted]				BIT					NOT	NULL
	,	[Name]					NVARCHAR(50)		NOT	NULL
	)

	INSERT
	INTO	@Inserted
	SELECT	[ID]
		,	[UID]
		,	[IsDeleted]
		,	[Name]
	FROM	INSERTED
	EXCEPT
	SELECT	[ID]
		,	[UID]
		,	[IsDeleted]
		,	[Name]
	FROM	DELETED

	SELECT	@Count = COUNT(*)
	FROM	@Inserted

	IF @Count > 0
	BEGIN
		SELECT	@Revision = dbo._Sys_GetSequenceNextValue('dbo', 'Revision')

		UPDATE	t
			SET	t.Revision = @Revision
		FROM	[dbo].[Objects] AS t
				INNER JOIN @Inserted AS i
					ON t.[UID] = i.[UID]
	END
END
GO

CREATE TRIGGER [dbo].[TR_Object_SetIsDeleted]
	ON	[dbo].[Objects]
	INSTEAD
		OF	DELETE
	NOT FOR REPLICATION
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE	t
		SET	t.IsDeleted = 1
	FROM	[dbo].[Objects] AS t
			INNER JOIN DELETED AS d
				ON	t.[UID] = d.[UID]
END
