CREATE TABLE [dbo].[CommentLikes] (
    [Id]             INT                NOT NULL,
    [LikingUserId]   INT                NOT NULL,
    [LikedCommentId] INT                NOT NULL,
    [LikeDateTime]   DATETIMEOFFSET (7) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT FK_CommentLikingUser FOREIGN KEY (LikingUserId) REFERENCES Users(Id),
	CONSTRAINT FK_LikedComment FOREIGN KEY (LikedCommentId) REFERENCES Comments(Id)
);

