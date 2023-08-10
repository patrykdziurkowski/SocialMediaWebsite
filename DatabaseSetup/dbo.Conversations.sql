CREATE TABLE [dbo].[Conversations]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Title] NVARCHAR(20) NOT NULL, 
    [Description] NVARCHAR(256) NULL, 
    [OwnerUserId] INT NOT NULL, 
    [CreationDateTime] DATETIMEOFFSET NOT NULL,
    CONSTRAINT FK_ConversationOwner FOREIGN KEY (OwnerUserId) REFERENCES Users(Id)
)
