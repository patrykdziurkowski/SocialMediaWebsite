--CREATE SCHEMA Tests;
GO
CREATE OR ALTER VIEW Tests.ViewWithRandomValue
AS
SELECT RAND() AS 'Value';

GO
CREATE OR ALTER FUNCTION Tests.GetRand()
RETURNS FLOAT
AS
BEGIN
	RETURN(SELECT [Value] FROM Tests.ViewWithRandomValue);
END;

GO
CREATE OR ALTER FUNCTION Tests.GetCharacterPool()
RETURNS NVARCHAR(128)
AS
BEGIN
	DECLARE @CharacterPool NVARCHAR(128) = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+[]{};'':"\|,.<>/?';
	RETURN(@CharacterPool);
END;

GO
CREATE OR ALTER FUNCTION Tests.GenerateSampleFakePasswordHash()
RETURNS NVARCHAR(256)
AS
BEGIN
	DECLARE @ReturnPasswordHash NVARCHAR(256) = '';
	DECLARE @CharacterPool NVARCHAR(128) = Tests.GetCharacterPool();

	DECLARE @PasswordLength INT = 0;
	WHILE @PasswordLength < 256
	BEGIN
		SET @PasswordLength = @PasswordLength + 1;

		DECLARE @RandomCharacter CHAR = SUBSTRING(@CharacterPool, CONVERT(int, Tests.GetRand() * LEN(@CharacterPool)) + 1, 1);
		SET @ReturnPasswordHash = @ReturnPasswordHash + @RandomCharacter;
	END;

	RETURN(@ReturnPasswordHash);
END;

GO
CREATE OR ALTER FUNCTION Tests.GenerateSampleFakeDate(@From DATETIMEOFFSET(7), @To DATETIMEOFFSET(7))
RETURNS DATETIMEOFFSET(7)
AS
BEGIN
	DECLARE @DateDifferenceInDays INT = (1 + DATEDIFF(DAY, @From, @To));
	RETURN(DATEADD(DAY, Tests.GetRand() * @DateDifferenceInDays, @From));
END;

GO
CREATE OR ALTER PROCEDURE Tests.InsertSampleData
AS
BEGIN
	
	DELETE FROM dbo.MessageLikes;
	DELETE FROM dbo.[Messages];
	DELETE FROM dbo.ConversationUsers;
	DELETE FROM dbo.Conversations;
	DELETE FROM dbo.CommentLikes;
	DELETE FROM dbo.Comments;
	DELETE FROM dbo.PostLikes;
	DELETE FROM dbo.Posts;
	DELETE FROM dbo.UserGroupMemberships;
	DELETE FROM dbo.GroupInvitations;
	DELETE FROM dbo.GroupApplications;
	DELETE FROM dbo.Groups;
	DELETE FROM dbo.GroupJoinRestrictions;
	DELETE FROM dbo.ProfileLikes;
	DELETE FROM dbo.Profiles;
	DELETE FROM dbo.Visibility;
	DELETE FROM dbo.BlockedUsers;
	DELETE FROM dbo.Users;
	
	



	DECLARE @AppStartDate DATETIMEOFFSET(7) = '2018-06-17 11:25:39';

	INSERT INTO Users 
	(UserName, FirstName, LastName, Email, IsEmailConfirmed, PasswordHash, JoinDateTime)
	VALUES
	('JohnSmith', 'John', 'Smith', 'jsmith@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	('HollieJames123', 'Hollie', 'James', 'hollieJ@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	('Cargigs', 'Carmen', 'Rigs', 'criggs@someemail.net', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	('ZakiHainz', 'Zaki', 'Haines', 'hainzZ@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	('RichardP', 'Richard', 'Pollard', 'rpollard@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	('RicardooO', 'Ricardo', 'Horton', 'ricardoHorton@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	('AngelaElliott423', 'Angela', 'Elliott', 'angelaElliott@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	('JoeSmith', 'Joe', 'Smith', 'joesmith@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	('Grant9423', 'Ashwin', 'Grant', 'agrant123@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	('anonymousDude123', NULL, NULL, 'anonymousDude@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	('AleenaM', NULL, 'Morrow', 'aleena.morrow@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	('JohnSmith1', 'John', 'Smith', 'otherJsmith@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	('ValentinaDillon1', NULL, NULL, 'valentina.1@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()));

END;