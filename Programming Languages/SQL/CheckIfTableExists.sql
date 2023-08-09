IF not exists (SELECT * FROM sysobjects WHERE name='FollowUp' and xtype='U')
BEGIN
	--CREATE TABLE [dbo].[FollowUp]
END
GO