
/* Create table for storing saved queries */
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[SavedQueryMeta](
	[QueryId] [int] IDENTITY(1,1) NOT NULL,
	[DataSourceType] [varchar](10) NOT NULL,
	[DatabaseId] [varchar](500) NULL,
	[DataSourceId] [varchar](500) NOT NULL,
	[Status] [char](1) NOT NULL,
	[StatusUse] [char](1) NOT NULL,
	[StatusChange] [char](1) NOT NULL,
	[OwnerId] [varchar](80) NOT NULL,
	[MyDescription] [varchar](250) NOT NULL,
	[Tags] [varchar](250) NULL,
	[CreatedDate] [smalldatetime] NOT NULL,
	[ChangedDate] [smalldatetime] NULL,
	[ChangedBy] [varchar](80) NULL,
	[UsedDate] [smalldatetime] NULL,
	[DataSourceUpdateDate] [smalldatetime] NULL,
	[SavedQueryFormat] [varchar](10) NOT NULL,
	[SavedQueryStorage] [char](1) NOT NULL,
	[QueryText] [varchar](max) NOT NULL,
	[Runs] [int] NOT NULL,
	[Fails] [int] NOT NULL,
 CONSTRAINT [PK_SavedQueryMeta] PRIMARY KEY CLUSTERED 
(
	[QueryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Automatically created id.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SavedQueryMeta', @level2type=N'COLUMN',@level2name=N'QueryId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nordic Meta Model and PX file.
Later on other format such as CoSSI.
CNMM = Common Nordic Meta Model database
PX = PC-Axis database.
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SavedQueryMeta', @level2type=N'COLUMN',@level2name=N'DataSourceType'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'In case of SQL this is the identifier for the database defined in the SQLDbConfig file.
In case of  PX points to the folder that makes up the PX database.
Enables metadata for the Saved Query to belong to a different database then the metadata database
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SavedQueryMeta', @level2type=N'COLUMN',@level2name=N'DatabaseId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'In case of SQL the id for the main table.
In case of  PX the relative path to the PX file.
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SavedQueryMeta', @level2type=N'COLUMN',@level2name=N'DataSourceId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Status for the Saved Query.
A = Active, is set by e.g. PX-Web when the query is created.
I = Inactive, can not be executed. Data source could be corrupt or the query could be corrupt. I set by the user.
S = Suspect, active but the metadata and/or data är ändrade is changed in the data source. Which could lead to possible problems? Is updated by MakroMeta, MakroData or OffData at SCB.
B = Broken, Is not able to run because of the Datasource no longer exists. Is set by the cleaning routine.
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SavedQueryMeta', @level2type=N'COLUMN',@level2name=N'Status'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Determines who could use the Saved Query.
O=Official = Everyone can use it (set by NSI)
P=Private = Only owner (OwnerId) can set it
I=Internal = Can only be used from internal
D=Shared = Everyone can use it . Is set by the owner/OwnerId
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SavedQueryMeta', @level2type=N'COLUMN',@level2name=N'StatusUse'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'States who can change the query.
P=Private = only the user (OwnerId) can change it
I=Internal = Can only be changed by the responsible for the maintable. (N/A in case of PX file database)
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SavedQueryMeta', @level2type=N'COLUMN',@level2name=N'StatusChange'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID for the user who have created the Saved Query. Can be a user or a group of users.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SavedQueryMeta', @level2type=N'COLUMN',@level2name=N'OwnerId'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Title or description of the Saved Query.
Is displayed for the user. Compar with PresText for the maintable or the TITLE in the PX file.
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SavedQueryMeta', @level2type=N'COLUMN',@level2name=N'MyDescription'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Keywords separated with a comma sign.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SavedQueryMeta', @level2type=N'COLUMN',@level2name=N'Tags'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date for when this row was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SavedQueryMeta', @level2type=N'COLUMN',@level2name=N'CreatedDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date for when this row last was changed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SavedQueryMeta', @level2type=N'COLUMN',@level2name=N'ChangedDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id of who made the changes. Could be OwerId or an application.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SavedQueryMeta', @level2type=N'COLUMN',@level2name=N'ChangedBy'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date when the query was last executed.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SavedQueryMeta', @level2type=N'COLUMN',@level2name=N'UsedDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date for when the datasource was update. Metadata or data was updated and Status is set to S.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SavedQueryMeta', @level2type=N'COLUMN',@level2name=N'DataSourceUpdateDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PXSXML
PXS
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SavedQueryMeta', @level2type=N'COLUMN',@level2name=N'SavedQueryFormat'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Storage space for the Saved Query. 
F = File system
D = Database
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SavedQueryMeta', @level2type=N'COLUMN',@level2name=N'SavedQueryStorage'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Path to the Saved Query or The Saved Query itself.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SavedQueryMeta', @level2type=N'COLUMN',@level2name=N'QueryText'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Number of times the saved query has been executed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SavedQueryMeta', @level2type=N'COLUMN',@level2name=N'Runs'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Number of times the execution of the saved query has failed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SavedQueryMeta', @level2type=N'COLUMN',@level2name=N'Fails'
GO

