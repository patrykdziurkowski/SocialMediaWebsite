USE [SocialMediaWebsite]
GO

/****** Object: Table [dbo].[Users] Script Date: 10.08.2023 14:06:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users] (
    [Id]               INT            NOT NULL,
    [UserName]         NVARCHAR (50)  NOT NULL,
    [FirstName]        NVARCHAR (50)  NULL,
    [LastName]         NVARCHAR (50)  NULL,
    [Email]            NVARCHAR (319) NOT NULL,
    [IsEmailConfirmed] BIT            NOT NULL,
    [PasswordHash]     NVARCHAR (128) NOT NULL,
    [PasswordSalt]     NVARCHAR (128) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


