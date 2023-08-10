CREATE TABLE [dbo].[PostLikes]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [LikingUserId] INT NOT NULL, 
    [LikedPostId] INT NOT NULL, 
    [LikeDateTime] DATETIMEOFFSET NOT NULL,
    CONSTRAINT FK_PostLikingUser FOREIGN KEY (LikingUserId) REFERENCES Users(Id),
    CONSTRAINT FK_LikedPost FOREIGN KEY (LikedPostId) REFERENCES Posts(Id)
)
