USE [master]
GO

IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'Auction')
DROP DATABASE [Auction]
GO

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

CREATE TABLE Account
( 
	Id                   uniqueidentifier  NOT NULL ,
	FirstName            nvarchar(50)  NULL ,
	LastName             nvarchar(50)  NULL ,
	Rate                 tinyint  NULL 
)
go

ALTER TABLE Account
	ADD CONSTRAINT XPKUser PRIMARY KEY  CLUSTERED (Id ASC)
go

CREATE TABLE Category
( 
	Id                   tinyint  NOT NULL ,
	Name                 nvarchar(30)  NULL ,
	Icon                 nvarchar(30)  NULL 
)
go

ALTER TABLE Category
	ADD CONSTRAINT XPKCategories PRIMARY KEY  CLUSTERED (Id ASC)
go

CREATE TABLE CategoryFeature
( 
	Id                   uniqueidentifier  NOT NULL ,
	CategoryId           tinyint  NULL ,
	Name                 nvarchar(max)  NULL ,
	PosibleValues        xml  NULL 
)
go

ALTER TABLE CategoryFeature
	ADD CONSTRAINT XPKCategoryFeature PRIMARY KEY  CLUSTERED (Id ASC)
go

CREATE TABLE Feedback
( 
	Id                   uniqueidentifier  NOT NULL ,
	Rate                 tinyint  NULL ,
	Description          nvarchar(max)  NULL ,
	UserId               uniqueidentifier  NULL 
)
go

ALTER TABLE Feedback
	ADD CONSTRAINT XPKFeedback PRIMARY KEY  CLUSTERED (Id ASC)
go

CREATE TABLE Item
( 
	Id                   uniqueidentifier  NOT NULL ,
	Name                 nvarchar(80)  NULL ,
	MinBet               decimal(12,2)  NULL ,
	Description          nvarchar(max)  NULL ,
	DueDateTime          datetime  NULL ,
	IsAvailable          bit  NULL ,
	IsPayed              bit  NULL ,
	IsReceived           bit  NULL ,
	SellerId             uniqueidentifier  NULL ,
	BuyerId              uniqueidentifier  NULL ,
	HighestBetId         uniqueidentifier  NULL ,
	FeatureId            uniqueidentifier  NULL 
)
go

ALTER TABLE Item
	ADD CONSTRAINT XPKItem PRIMARY KEY  CLUSTERED (Id ASC)
go

CREATE TABLE Notification
( 
	Id                   uniqueidentifier  NOT NULL ,
	Message              nvarchar(max)  NULL ,
	ItemId               uniqueidentifier  NULL ,
	ReceiverId           uniqueidentifier  NULL 
)
go

ALTER TABLE Notification
	ADD CONSTRAINT XPKNotification PRIMARY KEY  CLUSTERED (Id ASC)
go

CREATE TABLE Rate
( 
	Id                   uniqueidentifier  NOT NULL ,
	Amount               decimal(12,2)  NULL ,
	ItemId               uniqueidentifier  NULL ,
	BuyerId              uniqueidentifier  NULL 
)
go

ALTER TABLE Rate
	ADD CONSTRAINT XPKBet PRIMARY KEY  CLUSTERED (Id ASC)
go

CREATE TABLE Specification
( 
	Id                   uniqueidentifier  NOT NULL ,
	SelectedValues       nvarchar(max)  NULL ,
	CategoryFeatureId    uniqueidentifier  NULL 
)
go

ALTER TABLE Specification
	ADD CONSTRAINT XPKCharacter PRIMARY KEY  CLUSTERED (Id ASC)
go


ALTER TABLE CategoryFeature
	ADD CONSTRAINT R_18 FOREIGN KEY (CategoryId) REFERENCES Category(Id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go


ALTER TABLE Feedback
	ADD CONSTRAINT R_20 FOREIGN KEY (UserId) REFERENCES Account(Id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go


ALTER TABLE Item
	ADD CONSTRAINT R_2 FOREIGN KEY (SellerId) REFERENCES Account(Id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go

ALTER TABLE Item
	ADD CONSTRAINT R_9 FOREIGN KEY (FeatureId) REFERENCES Specification(Id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go

ALTER TABLE Item
	ADD CONSTRAINT R_13 FOREIGN KEY (BuyerId) REFERENCES Account(Id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go

ALTER TABLE Item
	ADD CONSTRAINT R_17 FOREIGN KEY (HighestBetId) REFERENCES Rate(Id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go


ALTER TABLE Notification
	ADD CONSTRAINT R_5 FOREIGN KEY (ItemId) REFERENCES Item(Id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go

ALTER TABLE Notification
	ADD CONSTRAINT R_6 FOREIGN KEY (ReceiverId) REFERENCES Account(Id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go


ALTER TABLE Rate
	ADD CONSTRAINT R_14 FOREIGN KEY (ItemId) REFERENCES Item(Id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go

ALTER TABLE Rate
	ADD CONSTRAINT R_15 FOREIGN KEY (BuyerId) REFERENCES Account(Id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go


ALTER TABLE Specification
	ADD CONSTRAINT R_19 FOREIGN KEY (CategoryFeatureId) REFERENCES CategoryFeature(Id)
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go
