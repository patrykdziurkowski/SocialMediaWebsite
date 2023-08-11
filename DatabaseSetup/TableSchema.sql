
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

CREATE TABLE [dbo].[BlockedUsers] (
    [Id]             INT                NOT NULL,
    [BlockingUserId] INT                NOT NULL,
    [BlockedUserId]  INT                NOT NULL,
    [BlockDateTime]  DATETIMEOFFSET (7) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT FK_BlockingUser FOREIGN KEY (BlockingUserId) REFERENCES Users(Id),
	CONSTRAINT FK_BlockedUser FOREIGN KEY (BlockedUserId) REFERENCES Users(Id)
);

CREATE TABLE [dbo].[Visibility]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Visibility] NVARCHAR(20) NOT NULL
)

CREATE TABLE [dbo].[Profiles]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Description] NVARCHAR(255) NULL, 
    [ProfilePicture] IMAGE NULL, 
    [ProfileVisibilityId] INT NOT NULL,
    CONSTRAINT FK_ProfileVisibility FOREIGN KEY (ProfileVisibilityId) REFERENCES Visibility(Id)
)

CREATE TABLE [dbo].[ProfileLikes]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [LikingUserId] INT NOT NULL, 
    [LikedProfileId] INT NOT NULL, 
    [LikeDateTime] DATETIMEOFFSET NOT NULL,
    CONSTRAINT FK_ProfileLikingUser FOREIGN KEY (LikingUserId) REFERENCES Users(Id),
    CONSTRAINT FK_LikedProfile FOREIGN KEY (LikedProfileId) REFERENCES Profiles(Id)
)

CREATE TABLE [dbo].[GroupJoinRestrictions] (
    [Id]     INT           NOT NULL,
    [Restriction] NVARCHAR (20) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Groups] (
    [Id]                 INT             NOT NULL,
    [Name]               NVARCHAR (128)  NOT NULL,
    [Description]        NVARCHAR (1024) NULL,
    [VisibilityId]       INT             NOT NULL,
    [JoinRestrictionId]  INT             NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT FK_GroupVisibility FOREIGN KEY (VisibilityId) REFERENCES Visibility(Id),
    CONSTRAINT FK_JoinRestriction FOREIGN KEY (JoinRestrictionId) REFERENCES GroupJoinRestrictions(Id)
);

CREATE TABLE [dbo].[GroupInvitations] (
    [Id]             INT                NOT NULL,
    [InvitingUserId] INT                NOT NULL,
    [InvitedUserId]  INT                NOT NULL,
    [GroupId]        INT                NOT NULL,
    [Message]        NVARCHAR (1024)    NULL,
    [InviteDateTime] DATETIMEOFFSET (7) NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT FK_GroupInvitingUser FOREIGN KEY (InvitingUserId) REFERENCES Users(Id),
    CONSTRAINT FK_GroupInvitedUser FOREIGN KEY (InvitedUserId) REFERENCES Users(Id),
    CONSTRAINT FK_InvitationGroup FOREIGN KEY (GroupId) REFERENCES Groups(Id)
);

CREATE TABLE [dbo].[GroupApplications]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [ApplyingUserId] INT NOT NULL, 
    [GroupId] INT NOT NULL, 
    [Message] NVARCHAR(1024) NULL, 
    [AppliedDateTime] DATETIMEOFFSET NOT NULL,
    CONSTRAINT FK_ApplyingUser FOREIGN KEY (ApplyingUserId) REFERENCES Users(Id),
    CONSTRAINT FK_ApplicationGroup FOREIGN KEY (GroupId) REFERENCES Groups(Id)
)

CREATE TABLE [dbo].[UserGroupMemberships]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [UserId] INT NOT NULL, 
    [GroupId] INT NOT NULL, 
    [JoinDateTime] DATETIMEOFFSET NOT NULL, 
    [WasInvited] BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_MembershipUser FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT FK_MembershipGroup FOREIGN KEY (GroupId) REFERENCES Groups(Id)
)

CREATE TABLE [dbo].[Posts] (
    [Id]                  INT                NOT NULL,
    [Title]               NVARCHAR (40)      NOT NULL,
    [Description]         NVARCHAR (1024)    NULL,
    [PostDateTime]        DATETIMEOFFSET (7) NOT NULL,
    [AuthorUserId]        INT                NOT NULL,
    [GroupId]             INT                NULL,
    [IsCommentingEnabled] BIT                DEFAULT ((1)) NOT NULL,
    [SharedPostId] INT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT FK_PostAuthor FOREIGN KEY (AuthorUserId) REFERENCES Users(Id),
    CONSTRAINT FK_PostGroup FOREIGN KEY (GroupId) REFERENCES Groups(Id),
    CONSTRAINT FK_SharedPost FOREIGN KEY (SharedPostId) REFERENCES Posts(Id)
);

CREATE TABLE [dbo].[PostLikes]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [LikingUserId] INT NOT NULL, 
    [LikedPostId] INT NOT NULL, 
    [LikeDateTime] DATETIMEOFFSET NOT NULL,
    CONSTRAINT FK_PostLikingUser FOREIGN KEY (LikingUserId) REFERENCES Users(Id),
    CONSTRAINT FK_LikedPost FOREIGN KEY (LikedPostId) REFERENCES Posts(Id)
)

CREATE TABLE [dbo].[Comments] (
    [Id]              INT                NOT NULL,
    [Text]            NVARCHAR (1024)    NOT NULL,
    [CommentDateTime] DATETIMEOFFSET (7) NOT NULL,
    [ParentPostId]    INT                NOT NULL,
    [ParentCommentId] INT                NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT FK_CommentParentPost FOREIGN KEY (ParentPostId) REFERENCES Posts(Id),
    CONSTRAINT FK_CommentParentComment FOREIGN KEY (ParentCommentId) REFERENCES Comments(Id)
);

CREATE TABLE [dbo].[CommentLikes] (
    [Id]             INT                NOT NULL,
    [LikingUserId]   INT                NOT NULL,
    [LikedCommentId] INT                NOT NULL,
    [LikeDateTime]   DATETIMEOFFSET (7) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT FK_CommentLikingUser FOREIGN KEY (LikingUserId) REFERENCES Users(Id),
	CONSTRAINT FK_LikedComment FOREIGN KEY (LikedCommentId) REFERENCES Comments(Id)
);

CREATE TABLE [dbo].[Conversations]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Title] NVARCHAR(20) NOT NULL, 
    [Description] NVARCHAR(256) NULL, 
    [OwnerUserId] INT NOT NULL, 
    [CreationDateTime] DATETIMEOFFSET NOT NULL,
    CONSTRAINT FK_ConversationOwner FOREIGN KEY (OwnerUserId) REFERENCES Users(Id)
)

CREATE TABLE [dbo].[ConversationUsers] (
    [Id]             INT NOT NULL,
    [UserId]         INT NOT NULL,
    [ConversationId] INT NOT NULL,
    [ConversationIsHidden] BIT NOT NULL DEFAULT 0, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT FK_ConversationUser FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT FK_UserConversation FOREIGN KEY (ConversationId) REFERENCES Conversations(Id)
);

CREATE TABLE [dbo].[Messages]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [AuthorUserId] INT NOT NULL, 
    [Text] NVARCHAR(256) NOT NULL, 
    [MessageDateTime] DATETIMEOFFSET NOT NULL, 
    [ConversationId] INT NOT NULL, 
    [ReplyMessageId] INT NULL,
    CONSTRAINT FK_MessageAuthor FOREIGN KEY (AuthorUserId) REFERENCES Users(Id),
    CONSTRAINT FK_MessageConversation FOREIGN KEY (ConversationId) REFERENCES Conversations(Id),
    CONSTRAINT FK_ReplyMessage FOREIGN KEY (ReplyMessageId) REFERENCES [Messages](Id)
)

CREATE TABLE [dbo].[MessageLikes]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [LikingUserId] INT NOT NULL, 
    [LikedMessageId] INT NOT NULL, 
    [LikeDateTime] DATETIMEOFFSET NOT NULL,
    CONSTRAINT FK_MessageLikingUser FOREIGN KEY (LikingUserId) REFERENCES Users(Id),
    CONSTRAINT FK_LikedMessage FOREIGN KEY (LikedMessageId) REFERENCES [Messages](Id)
)
