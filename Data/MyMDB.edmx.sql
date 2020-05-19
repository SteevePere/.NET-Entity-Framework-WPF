
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/03/2019 10:06:57
-- Generated from EDMX file: C:\Users\makav\source\repos\MyMDB\MyMDB\MyMDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MyMDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_MovieReview]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reviews] DROP CONSTRAINT [FK_MovieReview];
GO
IF OBJECT_ID(N'[dbo].[FK_UserReview]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reviews] DROP CONSTRAINT [FK_UserReview];
GO
IF OBJECT_ID(N'[dbo].[FK_MovieGenre_Movie]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MovieGenre] DROP CONSTRAINT [FK_MovieGenre_Movie];
GO
IF OBJECT_ID(N'[dbo].[FK_MovieGenre_Genre]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MovieGenre] DROP CONSTRAINT [FK_MovieGenre_Genre];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Movies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Movies];
GO
IF OBJECT_ID(N'[dbo].[Genres]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Genres];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Reviews]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Reviews];
GO
IF OBJECT_ID(N'[dbo].[MovieGenre]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MovieGenre];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Movies'
CREATE TABLE [dbo].[Movies] (
    [MOV_ID] int IDENTITY(1,1) NOT NULL,
    [MOV_TITLE] nvarchar(max)  NOT NULL,
    [MOV_SYNOPSIS] nvarchar(max)  NULL
);
GO

-- Creating table 'Genres'
CREATE TABLE [dbo].[Genres] (
    [GNR_ID] int IDENTITY(1,1) NOT NULL,
    [GNR_LABEL] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [USR_ID] int IDENTITY(1,1) NOT NULL,
    [USR_EMAIL] nvarchar(max)  NOT NULL,
    [USR_PASSWORD] nvarchar(max)  NOT NULL,
    [USR_FIRST_NAME] nvarchar(max)  NOT NULL,
    [USR_LAST_NAME] nvarchar(max)  NOT NULL,
    [USR_GENDER] int  NOT NULL,
    [USR_ACTIVE] bit  NOT NULL,
    [USR_LAST_CONNECTION_DATETIME] datetime  NULL,
    [USR_CREATION_DATETIME] datetime  NOT NULL,
    [USR_EDIT_DATETIME] datetime  NOT NULL
);
GO

-- Creating table 'Reviews'
CREATE TABLE [dbo].[Reviews] (
    [REV_ID] int IDENTITY(1,1) NOT NULL,
    [REV_CONTENT] nvarchar(max)  NULL,
    [REV_RATING] int  NOT NULL,
    [Movie_MOV_ID] int  NOT NULL,
    [User_USR_ID] int  NOT NULL
);
GO

-- Creating table 'MovieGenre'
CREATE TABLE [dbo].[MovieGenre] (
    [Movies_MOV_ID] int  NOT NULL,
    [Genres_GNR_ID] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [MOV_ID] in table 'Movies'
ALTER TABLE [dbo].[Movies]
ADD CONSTRAINT [PK_Movies]
    PRIMARY KEY CLUSTERED ([MOV_ID] ASC);
GO

-- Creating primary key on [GNR_ID] in table 'Genres'
ALTER TABLE [dbo].[Genres]
ADD CONSTRAINT [PK_Genres]
    PRIMARY KEY CLUSTERED ([GNR_ID] ASC);
GO

-- Creating primary key on [USR_ID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([USR_ID] ASC);
GO

-- Creating primary key on [REV_ID] in table 'Reviews'
ALTER TABLE [dbo].[Reviews]
ADD CONSTRAINT [PK_Reviews]
    PRIMARY KEY CLUSTERED ([REV_ID] ASC);
GO

-- Creating primary key on [Movies_MOV_ID], [Genres_GNR_ID] in table 'MovieGenre'
ALTER TABLE [dbo].[MovieGenre]
ADD CONSTRAINT [PK_MovieGenre]
    PRIMARY KEY CLUSTERED ([Movies_MOV_ID], [Genres_GNR_ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Movie_MOV_ID] in table 'Reviews'
ALTER TABLE [dbo].[Reviews]
ADD CONSTRAINT [FK_MovieReview]
    FOREIGN KEY ([Movie_MOV_ID])
    REFERENCES [dbo].[Movies]
        ([MOV_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MovieReview'
CREATE INDEX [IX_FK_MovieReview]
ON [dbo].[Reviews]
    ([Movie_MOV_ID]);
GO

-- Creating foreign key on [User_USR_ID] in table 'Reviews'
ALTER TABLE [dbo].[Reviews]
ADD CONSTRAINT [FK_UserReview]
    FOREIGN KEY ([User_USR_ID])
    REFERENCES [dbo].[Users]
        ([USR_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserReview'
CREATE INDEX [IX_FK_UserReview]
ON [dbo].[Reviews]
    ([User_USR_ID]);
GO

-- Creating foreign key on [Movies_MOV_ID] in table 'MovieGenre'
ALTER TABLE [dbo].[MovieGenre]
ADD CONSTRAINT [FK_MovieGenre_Movie]
    FOREIGN KEY ([Movies_MOV_ID])
    REFERENCES [dbo].[Movies]
        ([MOV_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Genres_GNR_ID] in table 'MovieGenre'
ALTER TABLE [dbo].[MovieGenre]
ADD CONSTRAINT [FK_MovieGenre_Genre]
    FOREIGN KEY ([Genres_GNR_ID])
    REFERENCES [dbo].[Genres]
        ([GNR_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MovieGenre_Genre'
CREATE INDEX [IX_FK_MovieGenre_Genre]
ON [dbo].[MovieGenre]
    ([Genres_GNR_ID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------