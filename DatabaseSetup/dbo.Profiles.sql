CREATE TABLE [dbo].[Profiles]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Description] NVARCHAR(255) NULL, 
    [ProfilePicture] IMAGE NULL, 
    [ProfileVisibilityId] INT NOT NULL,
    CONSTRAINT FK_ProfileVisibility FOREIGN KEY (ProfileVisibilityId) REFERENCES Visibility(Id)
)
