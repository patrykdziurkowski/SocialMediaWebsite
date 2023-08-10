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
