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
