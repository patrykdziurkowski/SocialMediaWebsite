CREATE TABLE [dbo].[Messages]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [AuthorUserId] INT NOT NULL, 
    [Text] NVARCHAR(256) NOT NULL, 
    [MessageDateTime] DATETIMEOFFSET NOT NULL, 
    [ConversationId] INT NOT NULL, 
    [ReplyMessageId] INT NULL,
    CONSTRAINT FK_MessageAuthor FOREIGN KEY (AuthorUserId) REFERENCES Users(Id),
    CONSTRAINT FK_MessageConversation FOREIGN KEY (ConversationId) REFERENCES Conversations(Id),
    CONSTRAINT FK_ReplyMessage FOREIGN KEY (ReplyMessageId) REFERENCES [Messages](Id)
)
