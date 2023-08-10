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

