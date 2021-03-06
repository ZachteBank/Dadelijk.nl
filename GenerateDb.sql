/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2016 (13.0.4422)
    Source Database Engine Edition : Microsoft SQL Server Express Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2017
    Target Database Engine Edition : Microsoft SQL Server Standard Edition
    Target Database Engine Type : Standalone SQL Server
*/
USE [bram]
GO
/****** Object:  Trigger [trg_UpdateTimeEntryTagNewsItem]    Script Date: 18-12-2017 11:53:01 ******/
DROP TRIGGER [dbo].[trg_UpdateTimeEntryTagNewsItem]
GO
/****** Object:  Trigger [trg_UpdateTimeEntryTag]    Script Date: 18-12-2017 11:53:01 ******/
DROP TRIGGER [dbo].[trg_UpdateTimeEntryTag]
GO
/****** Object:  Trigger [trg_UpdateTimeEntryReaction]    Script Date: 18-12-2017 11:53:01 ******/
DROP TRIGGER [dbo].[trg_UpdateTimeEntryReaction]
GO
/****** Object:  Trigger [trg_UpdateTimeEntryNews]    Script Date: 18-12-2017 11:53:01 ******/
DROP TRIGGER [dbo].[trg_UpdateTimeEntryNews]
GO
/****** Object:  Trigger [trg_UpdateTimeEntry]    Script Date: 18-12-2017 11:53:01 ******/
DROP TRIGGER [dbo].[trg_UpdateTimeEntry]
GO
/****** Object:  StoredProcedure [dbo].[CountTagsByNewsItem]    Script Date: 18-12-2017 11:53:01 ******/
DROP PROCEDURE [dbo].[CountTagsByNewsItem]
GO
ALTER TABLE [dbo].[TagNewsItem] DROP CONSTRAINT [FK__TagNewsIt__tagId__6B24EA82]
GO
ALTER TABLE [dbo].[TagNewsItem] DROP CONSTRAINT [FK__TagNewsIt__newsI__6A30C649]
GO
ALTER TABLE [dbo].[Reaction] DROP CONSTRAINT [FK__Reaction__reacti__5EBF139D]
GO
ALTER TABLE [dbo].[Reaction] DROP CONSTRAINT [FK__Reaction__newsIt__5CD6CB2B]
GO
ALTER TABLE [dbo].[Reaction] DROP CONSTRAINT [FK__Reaction__accoun__5DCAEF64]
GO
ALTER TABLE [dbo].[TagNewsItem] DROP CONSTRAINT [DF_TagNewsItem_dateCreated]
GO
ALTER TABLE [dbo].[Tag] DROP CONSTRAINT [DF_Tag_dateCreated]
GO
ALTER TABLE [dbo].[Reaction] DROP CONSTRAINT [DF_Reaction_dateCreated]
GO
ALTER TABLE [dbo].[Reaction] DROP CONSTRAINT [DF_Reaction_active]
GO
ALTER TABLE [dbo].[NewsItem] DROP CONSTRAINT [DF_NewsItem_dateCreated]
GO
ALTER TABLE [dbo].[NewsItem] DROP CONSTRAINT [DF_NewsItem_active]
GO
ALTER TABLE [dbo].[Account] DROP CONSTRAINT [DF_Account_dateCreated]
GO
ALTER TABLE [dbo].[Account] DROP CONSTRAINT [DF_Account_disabled]
GO
ALTER TABLE [dbo].[Account] DROP CONSTRAINT [DF_Account_accountType]
GO
/****** Object:  Table [dbo].[TagNewsItem]    Script Date: 18-12-2017 11:53:01 ******/
DROP TABLE [dbo].[TagNewsItem]
GO
/****** Object:  Table [dbo].[Tag]    Script Date: 18-12-2017 11:53:01 ******/
DROP TABLE [dbo].[Tag]
GO
/****** Object:  Table [dbo].[Reaction]    Script Date: 18-12-2017 11:53:01 ******/
DROP TABLE [dbo].[Reaction]
GO
/****** Object:  Table [dbo].[NewsItem]    Script Date: 18-12-2017 11:53:01 ******/
DROP TABLE [dbo].[NewsItem]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 18-12-2017 11:53:01 ******/
DROP TABLE [dbo].[Account]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 18-12-2017 11:53:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[email] [varchar](255) NOT NULL,
	[passHash] [varchar](70) NOT NULL,
	[accountTypeId] [int] NOT NULL,
	[disabled] [bit] NOT NULL,
	[dateCreated] [datetime] NOT NULL,
	[dateUpdated] [datetime] NULL,
	[username] [varchar](255) NULL,
 CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NewsItem]    Script Date: 18-12-2017 11:53:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NewsItem](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[subject] [varchar](255) NOT NULL,
	[text] [varchar](max) NOT NULL,
	[active] [bit] NOT NULL,
	[dateCreated] [datetime] NOT NULL,
	[dateUpdated] [datetime] NULL,
 CONSTRAINT [PK_NewsItem] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Reaction]    Script Date: 18-12-2017 11:53:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reaction](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[newsItemId] [int] NOT NULL,
	[accountId] [int] NOT NULL,
	[reactionId] [int] NULL,
	[text] [varchar](max) NOT NULL,
	[active] [bit] NOT NULL,
	[dateCreated] [datetime] NOT NULL,
	[dateUpdated] [datetime] NULL,
 CONSTRAINT [PK_Reaction] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tag]    Script Date: 18-12-2017 11:53:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tag](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NOT NULL,
	[dateCreated] [datetime] NOT NULL,
	[dateUpdated] [datetime] NULL,
 CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TagNewsItem]    Script Date: 18-12-2017 11:53:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TagNewsItem](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[newsItemId] [int] NOT NULL,
	[tagId] [int] NOT NULL,
	[dateCreated] [datetime] NOT NULL,
	[dateUpdated] [datetime] NULL,
 CONSTRAINT [PK_TagNewsItem] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_accountType]  DEFAULT ((0)) FOR [accountTypeId]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_disabled]  DEFAULT ((0)) FOR [disabled]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_dateCreated]  DEFAULT (getdate()) FOR [dateCreated]
GO
ALTER TABLE [dbo].[NewsItem] ADD  CONSTRAINT [DF_NewsItem_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[NewsItem] ADD  CONSTRAINT [DF_NewsItem_dateCreated]  DEFAULT (getdate()) FOR [dateCreated]
GO
ALTER TABLE [dbo].[Reaction] ADD  CONSTRAINT [DF_Reaction_active]  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[Reaction] ADD  CONSTRAINT [DF_Reaction_dateCreated]  DEFAULT (getdate()) FOR [dateCreated]
GO
ALTER TABLE [dbo].[Tag] ADD  CONSTRAINT [DF_Tag_dateCreated]  DEFAULT (getdate()) FOR [dateCreated]
GO
ALTER TABLE [dbo].[TagNewsItem] ADD  CONSTRAINT [DF_TagNewsItem_dateCreated]  DEFAULT (getdate()) FOR [dateCreated]
GO
ALTER TABLE [dbo].[Reaction]  WITH CHECK ADD FOREIGN KEY([accountId])
REFERENCES [dbo].[Account] ([id])
GO
ALTER TABLE [dbo].[Reaction]  WITH CHECK ADD FOREIGN KEY([newsItemId])
REFERENCES [dbo].[NewsItem] ([id])
GO
ALTER TABLE [dbo].[Reaction]  WITH CHECK ADD FOREIGN KEY([reactionId])
REFERENCES [dbo].[Reaction] ([id])
GO
ALTER TABLE [dbo].[TagNewsItem]  WITH CHECK ADD FOREIGN KEY([newsItemId])
REFERENCES [dbo].[NewsItem] ([id])
GO
ALTER TABLE [dbo].[TagNewsItem]  WITH CHECK ADD FOREIGN KEY([tagId])
REFERENCES [dbo].[Tag] ([id])
GO
/****** Object:  StoredProcedure [dbo].[CountTagsByNewsItem]    Script Date: 18-12-2017 11:53:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CountTagsByNewsItem]
(
@newsItemId INT OUT,
@count INT OUT
)
AS
BEGIN
SELECT @newsItemId = a.newsItemId, @count = count(*) FROM [bram].[dbo].[TagNewsItem] as a GROUP BY (a.newsItemId)
END
GO
/****** Object:  Trigger [dbo].[trg_UpdateTimeEntry]    Script Date: 18-12-2017 11:53:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[trg_UpdateTimeEntry]
ON [dbo].[Account]
AFTER UPDATE
AS
    UPDATE dbo.Account
    SET dateUpdated = GETDATE()
    WHERE id IN (SELECT DISTINCT id FROM Inserted)


GO
ALTER TABLE [dbo].[Account] ENABLE TRIGGER [trg_UpdateTimeEntry]
GO
/****** Object:  Trigger [dbo].[trg_UpdateTimeEntryNews]    Script Date: 18-12-2017 11:53:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[trg_UpdateTimeEntryNews]
ON [dbo].[NewsItem]
AFTER UPDATE
AS
    UPDATE dbo.NewsItem
    SET dateUpdated = GETDATE()
    WHERE id IN (SELECT DISTINCT id FROM Inserted)


GO
ALTER TABLE [dbo].[NewsItem] ENABLE TRIGGER [trg_UpdateTimeEntryNews]
GO
/****** Object:  Trigger [dbo].[trg_UpdateTimeEntryReaction]    Script Date: 18-12-2017 11:53:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[trg_UpdateTimeEntryReaction]
ON [dbo].[Reaction]
AFTER UPDATE
AS
    UPDATE dbo.Reaction
    SET dateUpdated = GETDATE()
    WHERE id IN (SELECT DISTINCT id FROM Inserted)


GO
ALTER TABLE [dbo].[Reaction] ENABLE TRIGGER [trg_UpdateTimeEntryReaction]
GO
/****** Object:  Trigger [dbo].[trg_UpdateTimeEntryTag]    Script Date: 18-12-2017 11:53:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[trg_UpdateTimeEntryTag]
ON [dbo].[Tag]
AFTER UPDATE
AS
    UPDATE dbo.Tag
    SET dateUpdated = GETDATE()
    WHERE id IN (SELECT DISTINCT id FROM Inserted)


GO
ALTER TABLE [dbo].[Tag] ENABLE TRIGGER [trg_UpdateTimeEntryTag]
GO
/****** Object:  Trigger [dbo].[trg_UpdateTimeEntryTagNewsItem]    Script Date: 18-12-2017 11:53:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[trg_UpdateTimeEntryTagNewsItem]
ON [dbo].[TagNewsItem]
AFTER UPDATE
AS
    UPDATE dbo.TagNewsItem
    SET dateUpdated = GETDATE()
    WHERE id IN (SELECT DISTINCT id FROM Inserted)


GO
ALTER TABLE [dbo].[TagNewsItem] ENABLE TRIGGER [trg_UpdateTimeEntryTagNewsItem]
GO
