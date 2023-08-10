CREATE TABLE [dbo].[ProfileLikes]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [LikingUserId] INT NOT NULL, 
    [LikedProfileId] INT NOT NULL, 
    [LikeDateTime] DATETIMEOFFSET NOT NULL,
    CONSTRAINT FK_ProfileLikingUser FOREIGN KEY (LikingUserId) REFERENCES Users(Id),
    CONSTRAINT FK_LikedProfile FOREIGN KEY (LikedProfileId) REFERENCES Profiles(Id)
)
