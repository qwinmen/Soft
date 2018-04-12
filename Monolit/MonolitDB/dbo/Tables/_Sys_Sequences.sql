CREATE TABLE [dbo].[_Sys_Sequences]
(	[ID]			UNIQUEIDENTIFIER	NOT	NULL	CONSTRAINT [DF_Sys_Sequences_ID] DEFAULT (newid())
,	[SchemaName]	SYSNAME				NOT	NULL	CONSTRAINT [DF_Sys_Sequences_Schema] DEFAULT ('dbo')
,	[Name]			SYSNAME				NOT	NULL
,	[Increment]		BIGINT				NOT	NULL	CONSTRAINT [DF_Sys_Sequences_Increment] DEFAULT (1)
,	[LastValue]		BIGINT				NOT	NULL	CONSTRAINT [DF_Sys_Sequences_LastValue] DEFAULT (0)
,	CONSTRAINT [PK_Sys_Sequences] PRIMARY KEY NONCLUSTERED ([ID])
,	CONSTRAINT [UQ_Sys_Sequences] UNIQUE CLUSTERED ([SchemaName], [Name])
)
