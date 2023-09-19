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
        [Id]               UNIQUEIDENTIFIER NOT NULL,
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
        [Id]             UNIQUEIDENTIFIER   NOT NULL,
        [BlockingUserId] UNIQUEIDENTIFIER                NOT NULL,
        [BlockedUserId]  UNIQUEIDENTIFIER                NOT NULL,
        [BlockDateTime]  DATETIMEOFFSET (7) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        PRIMARY KEY CLUSTERED ([Id] ASC),
	    CONSTRAINT FK_BlockingUser FOREIGN KEY (BlockingUserId) REFERENCES Users(Id),
	    CONSTRAINT FK_BlockedUser FOREIGN KEY (BlockedUserId) REFERENCES Users(Id)
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[Visibility]
    (
	    [Id] INT NOT NULL PRIMARY KEY, 
        [Visibility] NVARCHAR(20) NOT NULL
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[Profiles]
    (
	    [UserId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
        [Description] NVARCHAR(255) NULL, 
        [ProfilePicture] IMAGE NULL, 
        [ProfileVisibilityId] INT NOT NULL,
        CONSTRAINT FK_ProfileVisibility FOREIGN KEY (ProfileVisibilityId) REFERENCES Visibility(Id)
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[ProfileLikes]
    (
	    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
        [LikingUserId] UNIQUEIDENTIFIER NOT NULL, 
        [LikedProfileId] UNIQUEIDENTIFIER NOT NULL, 
        [LikeDateTime] DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        CONSTRAINT FK_ProfileLikingUser FOREIGN KEY (LikingUserId) REFERENCES Users(Id),
        CONSTRAINT FK_LikedProfile FOREIGN KEY (LikedProfileId) REFERENCES Profiles(UserId)
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[GroupJoinRestrictions] (
        [Id]     INT           NOT NULL,
        [Restriction] NVARCHAR (20) NOT NULL,
        PRIMARY KEY CLUSTERED ([Id] ASC)
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[Groups] (
        [Id]                 UNIQUEIDENTIFIER             NOT NULL,
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
        [Id]             UNIQUEIDENTIFIER                NOT NULL,
        [InvitingUserId] UNIQUEIDENTIFIER                NOT NULL,
        [InvitedUserId]  UNIQUEIDENTIFIER                NOT NULL,
        [GroupId]        UNIQUEIDENTIFIER                NOT NULL,
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
	    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
        [ApplyingUserId] UNIQUEIDENTIFIER NOT NULL, 
        [GroupId] UNIQUEIDENTIFIER NOT NULL, 
        [Message] NVARCHAR(1024) NULL, 
        [AppliedDateTime] DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        CONSTRAINT FK_ApplyingUser FOREIGN KEY (ApplyingUserId) REFERENCES Users(Id),
        CONSTRAINT FK_ApplicationGroup FOREIGN KEY (GroupId) REFERENCES Groups(Id)
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[UserGroupMemberships]
    (
	    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
        [UserId] UNIQUEIDENTIFIER NOT NULL, 
        [GroupId] UNIQUEIDENTIFIER NOT NULL, 
        [JoinDateTime] DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(), 
        [WasInvited] BIT NOT NULL DEFAULT 0,
        CONSTRAINT FK_MembershipUser FOREIGN KEY (UserId) REFERENCES Users(Id),
        CONSTRAINT FK_MembershipGroup FOREIGN KEY (GroupId) REFERENCES Groups(Id)
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[Posts] (
        [Id]                  UNIQUEIDENTIFIER                NOT NULL,
        [Title]               NVARCHAR (40)      NOT NULL,
        [Description]         NVARCHAR (1024)    NULL,
        [PostDateTime]        DATETIMEOFFSET (7) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        [AuthorUserId]        UNIQUEIDENTIFIER                NOT NULL,
        [GroupId]             UNIQUEIDENTIFIER                NULL,
        [IsCommentingEnabled] BIT                DEFAULT 1 NOT NULL,
        [SharedPostId] UNIQUEIDENTIFIER NULL, 
        PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT FK_PostAuthor FOREIGN KEY (AuthorUserId) REFERENCES Users(Id),
        CONSTRAINT FK_PostGroup FOREIGN KEY (GroupId) REFERENCES Groups(Id),
        CONSTRAINT FK_SharedPost FOREIGN KEY (SharedPostId) REFERENCES Posts(Id)
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[PostLikes]
    (
	    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
        [LikingUserId] UNIQUEIDENTIFIER NOT NULL, 
        [LikedPostId] UNIQUEIDENTIFIER NOT NULL, 
        [LikeDateTime] DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        CONSTRAINT FK_PostLikingUser FOREIGN KEY (LikingUserId) REFERENCES Users(Id),
        CONSTRAINT FK_LikedPost FOREIGN KEY (LikedPostId) REFERENCES Posts(Id)
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[Comments] (
        [Id]              UNIQUEIDENTIFIER                NOT NULL,
        [Text]            NVARCHAR (1024)    NOT NULL,
        [CommentDateTime] DATETIMEOFFSET (7) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        [ParentPostId]    UNIQUEIDENTIFIER                NOT NULL,
        [ParentCommentId] UNIQUEIDENTIFIER                NULL,
        [AuthorUserId] UNIQUEIDENTIFIER NOT NULL, 
        PRIMARY KEY CLUSTERED ([Id] ASC),
	    CONSTRAINT FK_CommentParentPost FOREIGN KEY (ParentPostId) REFERENCES Posts(Id),
        CONSTRAINT FK_CommentParentComment FOREIGN KEY (ParentCommentId) REFERENCES Comments(Id),
	    CONSTRAINT [FK_CommentAuthorUser] FOREIGN KEY ([AuthorUserId]) REFERENCES Users([Id])
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[CommentLikes] (
        [Id]             UNIQUEIDENTIFIER   NOT NULL,
        [LikingUserId]   UNIQUEIDENTIFIER                NOT NULL,
        [LikedCommentId] UNIQUEIDENTIFIER                NOT NULL,
        [LikeDateTime]   DATETIMEOFFSET (7) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        PRIMARY KEY CLUSTERED ([Id] ASC),
	    CONSTRAINT FK_CommentLikingUser FOREIGN KEY (LikingUserId) REFERENCES Users(Id),
	    CONSTRAINT FK_LikedComment FOREIGN KEY (LikedCommentId) REFERENCES Comments(Id)
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[Conversations]
    (
	    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
        [Title] NVARCHAR(20) NOT NULL, 
        [Description] NVARCHAR(256) NULL, 
        [OwnerUserId] UNIQUEIDENTIFIER NOT NULL, 
        [CreationDateTime] DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
        CONSTRAINT FK_ConversationOwner FOREIGN KEY (OwnerUserId) REFERENCES Users(Id)
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[ConversationUsers] (
        [Id]             UNIQUEIDENTIFIER NOT NULL,
        [UserId]         UNIQUEIDENTIFIER NOT NULL,
        [ConversationId] UNIQUEIDENTIFIER NOT NULL,
        [ConversationIsHidden] BIT NOT NULL DEFAULT 0, 
        PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT FK_ConversationUser FOREIGN KEY (UserId) REFERENCES Users(Id),
        CONSTRAINT FK_UserConversation FOREIGN KEY (ConversationId) REFERENCES Conversations(Id)
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[Messages]
    (
	    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
        [AuthorUserId] UNIQUEIDENTIFIER NOT NULL, 
        [Text] NVARCHAR(256) NOT NULL, 
        [MessageDateTime] DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(), 
        [ConversationId] UNIQUEIDENTIFIER NOT NULL, 
        [ReplyMessageId] UNIQUEIDENTIFIER NULL,
        CONSTRAINT FK_MessageAuthor FOREIGN KEY (AuthorUserId) REFERENCES Users(Id),
        CONSTRAINT FK_MessageConversation FOREIGN KEY (ConversationId) REFERENCES Conversations(Id),
        CONSTRAINT FK_ReplyMessage FOREIGN KEY (ReplyMessageId) REFERENCES [Messages](Id)
    );

    CREATE TABLE SocialMediaWebsite.[dbo].[MessageLikes]
    (
	    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
        [LikingUserId] UNIQUEIDENTIFIER NOT NULL, 
        [LikedMessageId] UNIQUEIDENTIFIER NOT NULL, 
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