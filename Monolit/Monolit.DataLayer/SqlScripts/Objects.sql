/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [ID]
      ,[DocID]
      ,[UID]
      ,[IsDeleted]
      ,[Revision]
      ,[Name]
  FROM [MonolitDB].[dbo].[Objects]

  /*
  INSERT INTO [MonolitDB].[dbo].[Objects]
  ([Name]) VALUES ('Sky')
  */