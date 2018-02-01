/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
:r .\AllData\dbo\Sys_Sequences.sql

EXEC [dbo].[_Sys_EnsureSequence] 'dbo', 'Revision'
EXEC [dbo].[_Sys_EnsureSequence] 'dbo', 'ObjectID'

GO

-- next script should be called last in any case
:r .\FixSequences.sql
