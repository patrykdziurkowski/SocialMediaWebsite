CREATE TABLE [dbo].[ConversationUsers] (
    [Id]             INT NOT NULL,
    [UserId]         INT NOT NULL,
    [ConversationId] INT NOT NULL,
    [ConversationIsHidden] BIT NOT NULL DEFAULT 0, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT FK_ConversationUser FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT FK_UserConversation FOREIGN KEY (ConversationId) REFERENCES Conversations(Id)
);

