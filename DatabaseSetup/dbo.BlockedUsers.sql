CREATE TABLE [dbo].[BlockedUsers] (
    [Id]             INT                NOT NULL,
    [BlockingUserId] INT                NOT NULL,
    [BlockedUserId]  INT                NOT NULL,
    [BlockDateTime]  DATETIMEOFFSET (7) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT FK_BlockingUser FOREIGN KEY (BlockingUserId) REFERENCES Users(Id),
	CONSTRAINT FK_BlockedUser FOREIGN KEY (BlockedUserId) REFERENCES Users(Id)
);

