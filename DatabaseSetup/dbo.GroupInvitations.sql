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

