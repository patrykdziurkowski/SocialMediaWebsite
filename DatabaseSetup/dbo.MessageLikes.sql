CREATE TABLE [dbo].[MessageLikes]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [LikingUserId] INT NOT NULL, 
    [LikedMessageId] INT NOT NULL, 
    [LikeDateTime] DATETIMEOFFSET NOT NULL,
    CONSTRAINT FK_MessageLikingUser FOREIGN KEY (LikingUserId) REFERENCES Users(Id),
    CONSTRAINT FK_LikedMessage FOREIGN KEY (LikedMessageId) REFERENCES [Messages](Id)
)
