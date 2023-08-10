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

