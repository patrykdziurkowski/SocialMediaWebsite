CREATE TABLE [dbo].[Groups] (
    [Id]                 INT             NOT NULL,
    [Name]               NVARCHAR (128)  NOT NULL,
    [Description]        NVARCHAR (1024) NULL,
    [VisibilityId]       INT             NOT NULL,
    [JoinRestrictionId]  INT             NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT FK_GroupVisibility FOREIGN KEY (VisibilityId) REFERENCES Visibility(Id),
    CONSTRAINT FK_JoinRestriction FOREIGN KEY (JoinRestrictionId) REFERENCES GroupJoinRestrictions(Id)
);

