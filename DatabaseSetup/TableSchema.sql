USE MASTER;
IF DB_ID (N'SocialMediaWebsite') IS NOT NULL
BEGIN
    
    ALTER DATABASE SocialMediaWebsite SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE SocialMediaWebsite;
END

CREATE DATABASE SocialMediaWebsite;
GO

BEGIN TRANSACTION SchemaSetupTransaction;
BEGIN TRY

    DROP TABLE IF EXISTS SocialMediaWebsite.dbo.MessageLikes;
    DROP TABLE IF EXISTS SocialMediaWebsite.dbo.[Messages];
    DROP TABLE IF EXISTS SocialMediaWebsite.dbo.ConversationUsers;
    DROP TABLE IF EXISTS SocialMediaWebsite.dbo.Conversations;
    DROP TABLE IF EXISTS SocialMediaWebsite.dbo.CommentLikes;
    DROP TABLE IF EXISTS SocialMediaWebsite.dbo.Comments;
    DROP TABLE IF EXISTS SocialMediaWebsite.dbo.PostLikes;
    DROP TABLE IF EXISTS SocialMediaWebsite.dbo.Posts;
    DROP TABLE IF EXISTS SocialMediaWebsite.dbo.UserGroupMemberships;
    DROP TABLE IF EXISTS SocialMediaWebsite.dbo.GroupApplications;
    DROP TABLE IF EXISTS SocialMediaWebsite.dbo.GroupInvitations;
    DROP TABLE IF EXISTS SocialMediaWebsite.dbo.Groups;
    DROP TABLE IF EXISTS SocialMediaWebsite.dbo.GroupJoinRestrictions;
    DROP TABLE IF EXISTS SocialMediaWebsite.dbo.ProfileLikes;
    DROP TABLE IF EXISTS SocialMediaWebsite.dbo.Profiles;
    DROP TABLE IF EXISTS SocialMediaWebsite.dbo.Visibility;
    DROP TABLE IF EXISTS SocialMediaWebsite.dbo.BlockedUsers;
    DROP TABLE IF EXISTS SocialMediaWebsite.dbo.Users;


    CREATE TABLE SocialMediaWebsite.[dbo].[Users] (
        [Id]               INT            NOT NULL IDENTITY(1,1),
        [UserName]         NVARCHAR (50)  NOT NULL,
        [FirstName]        NVARCHAR (50)  NULL,
        [LastName]         NVARCHAR (50)  NULL,
        [Email]            NVARCHAR (319) NOT NULL,
        [IsEmailConfirmed] BIT            NOT NULL DEFAULT 0,
        [PasswordHash]     NVARCHAR (256) NOT NULL,
        [JoinDateTime]  DATETIMEOFFSET (7) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        PRIMARY KEY CLUSTERED ([Id] ASC)
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[BlockedUsers] (
        [Id]             INT                NOT NULL IDENTITY(1,1),
        [BlockingUserId] INT                NOT NULL,
        [BlockedUserId]  INT                NOT NULL,
        [BlockDateTime]  DATETIMEOFFSET (7) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        PRIMARY KEY CLUSTERED ([Id] ASC),
	    CONSTRAINT FK_BlockingUser FOREIGN KEY (BlockingUserId) REFERENCES Users(Id),
	    CONSTRAINT FK_BlockedUser FOREIGN KEY (BlockedUserId) REFERENCES Users(Id)
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[Visibility]
    (
	    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
        [Visibility] NVARCHAR(20) NOT NULL
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[Profiles]
    (
	    [UserId] INT NOT NULL PRIMARY KEY, 
        [Description] NVARCHAR(255) NULL, 
        [ProfilePicture] IMAGE NULL, 
        [ProfileVisibilityId] INT NOT NULL,
        CONSTRAINT FK_ProfileVisibility FOREIGN KEY (ProfileVisibilityId) REFERENCES Visibility(Id)
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[ProfileLikes]
    (
	    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
        [LikingUserId] INT NOT NULL, 
        [LikedProfileId] INT NOT NULL, 
        [LikeDateTime] DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        CONSTRAINT FK_ProfileLikingUser FOREIGN KEY (LikingUserId) REFERENCES Users(Id),
        CONSTRAINT FK_LikedProfile FOREIGN KEY (LikedProfileId) REFERENCES Profiles(UserId)
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[GroupJoinRestrictions] (
        [Id]     INT           NOT NULL IDENTITY(1,1),
        [Restriction] NVARCHAR (20) NOT NULL,
        PRIMARY KEY CLUSTERED ([Id] ASC)
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[Groups] (
        [Id]                 INT             NOT NULL IDENTITY(1,1),
        [Name]               NVARCHAR (128)  NOT NULL,
        [Description]        NVARCHAR (1024) NULL,
        [VisibilityId]       INT             NOT NULL,
        [JoinRestrictionId]  INT             NOT NULL,
        [IsActive]           BIT             NOT NULL DEFAULT 1,
        [CreationDateTime]  DATETIMEOFFSET (7) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT FK_GroupVisibility FOREIGN KEY (VisibilityId) REFERENCES Visibility(Id),
        CONSTRAINT FK_JoinRestriction FOREIGN KEY (JoinRestrictionId) REFERENCES GroupJoinRestrictions(Id)
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[GroupInvitations] (
        [Id]             INT                NOT NULL IDENTITY(1,1),
        [InvitingUserId] INT                NOT NULL,
        [InvitedUserId]  INT                NOT NULL,
        [GroupId]        INT                NOT NULL,
        [Message]        NVARCHAR (1024)    NULL,
        [InviteDateTime] DATETIMEOFFSET (7) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        [IsActive]       BIT                NOT NULL DEFAULT 1, 
        PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT FK_GroupInvitingUser FOREIGN KEY (InvitingUserId) REFERENCES Users(Id),
        CONSTRAINT FK_GroupInvitedUser FOREIGN KEY (InvitedUserId) REFERENCES Users(Id),
        CONSTRAINT FK_InvitationGroup FOREIGN KEY (GroupId) REFERENCES Groups(Id)
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[GroupApplications]
    (
	    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
        [ApplyingUserId] INT NOT NULL, 
        [GroupId] INT NOT NULL, 
        [Message] NVARCHAR(1024) NULL, 
        [AppliedDateTime] DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        CONSTRAINT FK_ApplyingUser FOREIGN KEY (ApplyingUserId) REFERENCES Users(Id),
        CONSTRAINT FK_ApplicationGroup FOREIGN KEY (GroupId) REFERENCES Groups(Id)
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[UserGroupMemberships]
    (
	    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
        [UserId] INT NOT NULL, 
        [GroupId] INT NOT NULL, 
        [JoinDateTime] DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(), 
        [WasInvited] BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_MembershipUser FOREIGN KEY (UserId) REFERENCES Users(Id),
        CONSTRAINT FK_MembershipGroup FOREIGN KEY (GroupId) REFERENCES Groups(Id)
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[Posts] (
        [Id]                  INT                NOT NULL IDENTITY(1,1),
        [Title]               NVARCHAR (40)      NOT NULL,
        [Description]         NVARCHAR (1024)    NULL,
        [PostDateTime]        DATETIMEOFFSET (7) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        [AuthorUserId]        INT                NOT NULL,
        [GroupId]             INT                NULL,
        [IsCommentingEnabled] BIT                DEFAULT 1 NOT NULL,
        [SharedPostId] INT NULL, 
        PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT FK_PostAuthor FOREIGN KEY (AuthorUserId) REFERENCES Users(Id),
        CONSTRAINT FK_PostGroup FOREIGN KEY (GroupId) REFERENCES Groups(Id),
        CONSTRAINT FK_SharedPost FOREIGN KEY (SharedPostId) REFERENCES Posts(Id)
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[PostLikes]
    (
	    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
        [LikingUserId] INT NOT NULL, 
        [LikedPostId] INT NOT NULL, 
        [LikeDateTime] DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        CONSTRAINT FK_PostLikingUser FOREIGN KEY (LikingUserId) REFERENCES Users(Id),
        CONSTRAINT FK_LikedPost FOREIGN KEY (LikedPostId) REFERENCES Posts(Id)
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[Comments] (
        [Id]              INT                NOT NULL IDENTITY(1,1),
        [Text]            NVARCHAR (1024)    NOT NULL,
        [CommentDateTime] DATETIMEOFFSET (7) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        [ParentPostId]    INT                NOT NULL,
        [ParentCommentId] INT                NULL,
        [AuthorUserId] INT NOT NULL, 
        PRIMARY KEY CLUSTERED ([Id] ASC),
	    CONSTRAINT FK_CommentParentPost FOREIGN KEY (ParentPostId) REFERENCES Posts(Id),
        CONSTRAINT FK_CommentParentComment FOREIGN KEY (ParentCommentId) REFERENCES Comments(Id),
	    CONSTRAINT [FK_CommentAuthorUser] FOREIGN KEY ([AuthorUserId]) REFERENCES Users([Id])
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[CommentLikes] (
        [Id]             INT                NOT NULL IDENTITY(1,1),
        [LikingUserId]   INT                NOT NULL,
        [LikedCommentId] INT                NOT NULL,
        [LikeDateTime]   DATETIMEOFFSET (7) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        PRIMARY KEY CLUSTERED ([Id] ASC),
	    CONSTRAINT FK_CommentLikingUser FOREIGN KEY (LikingUserId) REFERENCES Users(Id),
	    CONSTRAINT FK_LikedComment FOREIGN KEY (LikedCommentId) REFERENCES Comments(Id)
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[Conversations]
    (
	    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
        [Title] NVARCHAR(20) NOT NULL, 
        [Description] NVARCHAR(256) NULL, 
        [OwnerUserId] INT NOT NULL, 
        [CreationDateTime] DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        CONSTRAINT FK_ConversationOwner FOREIGN KEY (OwnerUserId) REFERENCES Users(Id)
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[ConversationUsers] (
        [Id]             INT NOT NULL IDENTITY(1,1),
        [UserId]         INT NOT NULL,
        [ConversationId] INT NOT NULL,
        [ConversationIsHidden] BIT NOT NULL DEFAULT 0, 
        PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT FK_ConversationUser FOREIGN KEY (UserId) REFERENCES Users(Id),
        CONSTRAINT FK_UserConversation FOREIGN KEY (ConversationId) REFERENCES Conversations(Id)
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[Messages]
    (
	    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
        [AuthorUserId] INT NOT NULL, 
        [Text] NVARCHAR(256) NOT NULL, 
        [MessageDateTime] DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(), 
        [ConversationId] INT NOT NULL, 
        [ReplyMessageId] INT NULL,
        CONSTRAINT FK_MessageAuthor FOREIGN KEY (AuthorUserId) REFERENCES Users(Id),
        CONSTRAINT FK_MessageConversation FOREIGN KEY (ConversationId) REFERENCES Conversations(Id),
        CONSTRAINT FK_ReplyMessage FOREIGN KEY (ReplyMessageId) REFERENCES [Messages](Id)
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[MessageLikes]
    (
	    [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
        [LikingUserId] INT NOT NULL, 
        [LikedMessageId] INT NOT NULL, 
        [LikeDateTime] DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        CONSTRAINT FK_MessageLikingUser FOREIGN KEY (LikingUserId) REFERENCES Users(Id),
        CONSTRAINT FK_LikedMessage FOREIGN KEY (LikedMessageId) REFERENCES [Messages](Id)
    );

    COMMIT TRANSACTION SchemaSetupTransaction;
    PRINT N'Finished creating the tables.';
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION SchemaSetupTransaction;
    PRINT N'Creating tables failed. Transaction rolled back.';
    THROW;
END CATCH