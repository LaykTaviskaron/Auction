USE [master]
GO

IF  EXISTS (SELECT name FROM sys.databases WHERE name = 'Auction')
ALTER DATABASE [Auction] set single_user with rollback immediate
DROP DATABASE [Auction]

USE [master]
GO

PRINT 'Creating Database...'
CREATE DATABASE [Auction]
GO
PRINT 'Database created successfully...'
GO
PRINT 'Selecting database instance...'
GO
USE [Auction]
GO
PRINT 'Creating database tables...'

GO
CREATE TABLE [Account]
( 
	[Id]                 uniqueidentifier  NOT NULL ,
	[FirstName]          nvarchar(50)  NOT NULL ,
	[LastName]           nvarchar(50)  NOT NULL ,
	[Rate]               tinyint  NULL ,
	[Email]              varchar(Max)  NOT NULL ,
	CONSTRAINT [XPKUser] PRIMARY KEY  CLUSTERED ([Id] ASC)
)
go

CREATE TABLE [Bet]
( 
	[Id]                 uniqueidentifier  NOT NULL ,
	[ItemId]             uniqueidentifier  NOT NULL ,
	[BuyerId]            uniqueidentifier  NOT NULL ,
	[Amout]              decimal(12,2)  NOT NULL ,
	[BetTypeId]          tinyint  NULL ,
	CONSTRAINT [XPKBet] PRIMARY KEY  CLUSTERED ([Id] ASC)
)
go

CREATE NONCLUSTERED INDEX [XIF1Bet] ON [Bet]
( 
	[ItemId]              ASC
)
go

CREATE NONCLUSTERED INDEX [XIF2Bet] ON [Bet]
( 
	[BuyerId]             ASC
)
go

CREATE NONCLUSTERED INDEX [XIF3Bet] ON [Bet]
( 
	[BetTypeId]           ASC
)
go

CREATE TABLE [BetType]
( 
	[Id]                 tinyint  NOT NULL ,
	[Name]               nvarchar(20)  NULL ,
	CONSTRAINT [XPKBetType] PRIMARY KEY  CLUSTERED ([Id] ASC)
)
go

CREATE TABLE [Category]
( 
	[Id]                 tinyint  NOT NULL ,
	[Name]               nvarchar(30)  NULL ,
	[Icon]               nvarchar(30)  NULL ,
	CONSTRAINT [XPKCategories] PRIMARY KEY  CLUSTERED ([Id] ASC)
)
go

CREATE TABLE [CategoryFeature]
( 
	[Id]                 uniqueidentifier  NOT NULL ,
	[Name]               nvarchar(max)  NULL ,
	[CategoryId]         tinyint  NULL ,
	[PosibleValues]      xml  NULL ,
	CONSTRAINT [XPKCategoryFeature] PRIMARY KEY  CLUSTERED ([Id] ASC)
)
go

CREATE NONCLUSTERED INDEX [XIF1CategoryFeature] ON [CategoryFeature]
( 
	[CategoryId]          ASC
)
go

CREATE TABLE [Feedback]
( 
	[Id]                 uniqueidentifier  NOT NULL ,
	[Rate]               tinyint  NOT NULL ,
	[Description]        nvarchar(max)  NOT NULL ,
	[UserId]             uniqueidentifier  NOT NULL ,
	CONSTRAINT [XPKFeedback] PRIMARY KEY  CLUSTERED ([Id] ASC)
)
go

CREATE NONCLUSTERED INDEX [XIF1Feedback] ON [Feedback]
( 
	[UserId]              ASC
)
go

CREATE TABLE [Item]
( 
	[Id]                 uniqueidentifier  NOT NULL ,
	[Name]               nvarchar(80)  NOT NULL ,
	[Description]        nvarchar(max)  NOT NULL ,
	[DueDateTime]        datetime  NOT NULL ,
	[IsAvailable]        bit  NULL ,
	[SellerId]           uniqueidentifier  NOT NULL ,
	[CategoryId]         tinyint  NOT NULL ,
	[MinBet]             decimal(12,2)  NOT NULL ,
	[IsPayed]            bit  NULL ,
	[IsReceived]         bit  NULL ,
	[BuyerId]            uniqueidentifier  NULL ,
	[HighestBetId]       uniqueidentifier  NULL ,
	[Image]              varchar(max)  NULL ,
	CONSTRAINT [XPKItem] PRIMARY KEY  CLUSTERED ([Id] ASC)
)
go

CREATE NONCLUSTERED INDEX [XIF1Item] ON [Item]
( 
	[SellerId]            ASC
)
go

CREATE NONCLUSTERED INDEX [XIF4Item] ON [Item]
( 
	[BuyerId]             ASC
)
go

CREATE NONCLUSTERED INDEX [XIF5Item] ON [Item]
( 
	[HighestBetId]        ASC
)
go

CREATE NONCLUSTERED INDEX [XIF2Item] ON [Item]
( 
	[CategoryId]          ASC
)
go

CREATE TABLE [Notification]
( 
	[Id]                 uniqueidentifier  NOT NULL ,
	[Message]            nvarchar(max)  NOT NULL ,
	[ItemId]             uniqueidentifier  NOT NULL ,
	[ReceiverId]         uniqueidentifier  NOT NULL ,
	[NotificationTypeId] tinyint  NULL ,
	CONSTRAINT [XPKNotification] PRIMARY KEY  CLUSTERED ([Id] ASC)
)
go

CREATE NONCLUSTERED INDEX [XIF1Notification] ON [Notification]
( 
	[ItemId]              ASC
)
go

CREATE NONCLUSTERED INDEX [XIF2Notification] ON [Notification]
( 
	[ReceiverId]          ASC
)
go

CREATE NONCLUSTERED INDEX [XIF3Notification] ON [Notification]
( 
	[NotificationTypeId]  ASC
)
go

CREATE TABLE [NotificcationType]
( 
	[Id]                 tinyint  NOT NULL ,
	[Name]               nvarchar(max)  NULL ,
	CONSTRAINT [XPKNotificcationType] PRIMARY KEY  CLUSTERED ([Id] ASC)
)
go

CREATE TABLE [Specifications]
( 
	[Id]                 uniqueidentifier  NOT NULL ,
	[SelectedValue]      nvarchar(max)  NULL ,
	[CategoryFeatureId]  uniqueidentifier  NULL ,
	[ItemId]             uniqueidentifier  NULL ,
	CONSTRAINT [XPKCharacter] PRIMARY KEY  CLUSTERED ([Id] ASC)
)
go

CREATE NONCLUSTERED INDEX [XIF1Character] ON [Specifications]
( 
	[CategoryFeatureId]   ASC
)
go

CREATE NONCLUSTERED INDEX [XIF2Character] ON [Specifications]
( 
	[ItemId]              ASC
)
go


ALTER TABLE [Bet]
	ADD CONSTRAINT [R_14] FOREIGN KEY ([ItemId]) REFERENCES [Item]([Id])
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go

ALTER TABLE [Bet]
	ADD CONSTRAINT [R_15] FOREIGN KEY ([BuyerId]) REFERENCES [Account]([Id])
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go

ALTER TABLE [Bet]
	ADD CONSTRAINT [R_16] FOREIGN KEY ([BetTypeId]) REFERENCES [BetType]([Id])
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go


ALTER TABLE [CategoryFeature]
	ADD CONSTRAINT [R_18] FOREIGN KEY ([CategoryId]) REFERENCES [Category]([Id])
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go


ALTER TABLE [Feedback]
	ADD CONSTRAINT [R_20] FOREIGN KEY ([UserId]) REFERENCES [Account]([Id])
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go


ALTER TABLE [Item]
	ADD CONSTRAINT [R_2] FOREIGN KEY ([SellerId]) REFERENCES [Account]([Id])
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go

ALTER TABLE [Item]
	ADD CONSTRAINT [R_13] FOREIGN KEY ([BuyerId]) REFERENCES [Account]([Id])
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go

ALTER TABLE [Item]
	ADD CONSTRAINT [R_17] FOREIGN KEY ([HighestBetId]) REFERENCES [Bet]([Id])
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go

ALTER TABLE [Item]
	ADD CONSTRAINT [R_3] FOREIGN KEY ([CategoryId]) REFERENCES [Category]([Id])
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go


ALTER TABLE [Notification]
	ADD CONSTRAINT [R_5] FOREIGN KEY ([ItemId]) REFERENCES [Item]([Id])
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go

ALTER TABLE [Notification]
	ADD CONSTRAINT [R_6] FOREIGN KEY ([ReceiverId]) REFERENCES [Account]([Id])
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go

ALTER TABLE [Notification]
	ADD CONSTRAINT [R_7] FOREIGN KEY ([NotificationTypeId]) REFERENCES [NotificcationType]([Id])
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go


ALTER TABLE [Specifications]
	ADD CONSTRAINT [R_19] FOREIGN KEY ([CategoryFeatureId]) REFERENCES [CategoryFeature]([Id])
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go

ALTER TABLE [Specifications]
	ADD CONSTRAINT [R_23] FOREIGN KEY ([ItemId]) REFERENCES [Item]([Id])
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go
