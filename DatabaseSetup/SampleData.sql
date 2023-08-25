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
	
		SET IDENTITY_INSERT dbo.Posts ON;
		INSERT INTO dbo.Posts
		(Id, Title, [Description], PostDateTime, AuthorUserId, GroupId, IsCommentingEnabled, SharedPostId)
		VALUES
		(1, N'Great day to work', N'Wowie, today is truly a great day to be working! Like this post if you agree!', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 1, 4, 1, NULL),
		(2, N'How''s your day', NULL, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 8, NULL, 0, NULL),
		(3, N'Mistake', N'You forgot to enable the comments! 💬💬💬❗❗', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 2, NULL, 1, 2),
		(4, N'Is anyone selling', N'selling a hacksaw??', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 8, 2, 1, NULL),
		(5, N'Lawn mower', N'I wish to buy a new lawn mower I got $100 to make it happen send me a message', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 8, 2, 1, NULL),
		(6, N'❗❗❗Used furtniture❗❗❗', N'Greetings, I wish to sell a lot of used furniture, please send me a message if you''re interested.', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 5, 2, 1, NULL),
		(7, N'Group rules:📝', N'Will fill out in the future', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 5, 5, 0, NULL),
		(8, N'did u see the news?', N'i saw the news', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 10, NULL, 1, NULL),
		(9, N'Some people are just clueless', N'Thoughts?', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 2, NULL, 1, NULL),
		(10, N'Let''s create a chain', N'of quote posts', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 11, 3, 1, NULL),
		(11, N'adaasdasvcxklxcvvzxcxcvxcvm,xcvz1234#', NULL, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 12, 3, 1, 11),
		(12, N'Stop spamming', N'Or I''m going to kick you out', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 13, 3, 1, 12),
		(13, N'Events', N'Is anything planned for tomorrow?', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 3, 1, 1, NULL),
		(14, N'Invitation to SomeGroup', N'Could someone please invite me to SomeGroup?', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 6, NULL, 1, NULL),
		(15, N'Boring', N'Man this platform is dead...', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 10, NULL, 1, NULL),
		(16, N'any1 wanna talk cars', 'i love cars', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 8, NULL, 1, NULL);
		SET IDENTITY_INSERT dbo.Posts OFF;
	



	DECLARE @AppStartDate DATETIMEOFFSET(7) = '2018-06-17 11:25:39';

	SET IDENTITY_INSERT dbo.Visibility ON;
	INSERT INTO dbo.Visibility
	(Id, Visibility)
	VALUES
	(1, 'Public'),
	(2, 'Hidden');
	SET IDENTITY_INSERT dbo.Visibility OFF;

	SET IDENTITY_INSERT dbo.Users ON;
	INSERT INTO dbo.Users 
	(Id, UserName, FirstName, LastName, Email, IsEmailConfirmed, PasswordHash, JoinDateTime)
	VALUES
	(1, 'JohnSmith', 'John', 'Smith', 'jsmith@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(2, 'HollieJames123', 'Hollie', 'James', 'hollieJ@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(3, 'Cargigs', 'Carmen', 'Rigs', 'criggs@someemail.net', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(4, 'ZakiHainz', 'Zaki', 'Haines', 'hainzZ@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(5, 'RichardP', 'Richard', 'Pollard', 'rpollard@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(6, 'RicardooO', 'Ricardo', 'Horton', 'ricardoHorton@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(7, 'AngelaElliott423', 'Angela', 'Elliott', 'angelaElliott@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(8, 'JoeSmith', 'Joe', 'Smith', 'joesmith@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(9, 'Grant9423', 'Ashwin', 'Grant', 'agrant123@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(10, 'anonymousDude123', NULL, NULL, 'anonymousDude@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(11, 'AleenaM', NULL, 'Morrow', 'aleena.morrow@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(12, 'JohnSmith1', 'John', 'Smith', 'otherJsmith@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(13, 'ValentinaDillon1', NULL, NULL, 'valentina.1@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()));
	SET IDENTITY_INSERT dbo.Users OFF;

	SET IDENTITY_INSERT dbo.Conversations ON;
	INSERT INTO dbo.Conversations
	(Id, Title, [Description], OwnerUserId, CreationDateTime)
	VALUES
	(1, 'WorkConversation', 'This is our conversation meant for work purposes only', 1, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(2, 'FunConvo', NULL, 6, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(3, 'GroupConversation1', NULL, 6, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(4, 'FamilyConversation', 'Family@', 8, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(5, 'GroupConversation1', NULL, 9, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()));
	SET IDENTITY_INSERT dbo.Conversations OFF;

	SET IDENTITY_INSERT dbo.ConversationUsers ON;
	INSERT INTO dbo.ConversationUsers
	(Id, ConversationId, UserId, ConversationIsHidden)
	VALUES
	(1, 1, 1, 0),
	(2, 1, 7, 0),
	(3, 1, 11, 0),
	(4, 1, 12, 0),

	(5, 2, 13, 0),
	(6, 2, 12, 0),
	(7, 2, 11, 0),
	(8, 2, 10, 0),
	(9, 2, 6, 0),

	(10, 3, 6, 0),
	(11, 3, 12, 0),

	(12, 4, 1, 1),
	(13, 4, 8, 0),

	(14, 5, 6, 0),
	(15, 5, 9, 0);
	SET IDENTITY_INSERT dbo.ConversationUsers OFF;

	SET IDENTITY_INSERT dbo.[Messages] ON;
	INSERT INTO dbo.[Messages]
	(Id, AuthorUserId, [Text], MessageDateTime, Conversationid, ReplyMessageId)
	VALUES
	(1, 1, N'Hello everyone!', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 1, NULL),
	(2, 1, N'Let me know once everyone is here', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 1, 1),
	(3, 7, N'Hi John I''m here! 👋', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 1, 2),
	(4, 11, N'Howdy', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 1, NULL),
	(5, 12, N'When are we getting started?', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 1, 1),

	(6, 13, N'does this thing work', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 2, NULL),
	(7, 13, N'hello anybody there? 🥱', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 2, NULL),
	(8, 13, N'my internet must be out...', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 2, NULL),
	(9, 12, N'Stop spamming', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 2, 8),

	(10, 6, N'Yo', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 3, NULL),
	(11, 6, N'Wanna come to the party', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 3, NULL),
	(12, 12, N'I don''t know man', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 3, NULL),
	(13, 6, N'its gonna be fun', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 3, NULL),
	(14, 12, N'ok', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 3, NULL),
	(15, 12, N' ', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 3, NULL),

	(16, 8, N'Hi John how''s work', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 4, NULL),
	(17, 1, N'Why it is going very well, now please do not bother me', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 4, NULL),
	(18, 8, N'Did you see my lawn mower I can''t fidn it', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 4, NULL),
	(19, 8, N'I need to mow the grass', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 4, NULL),
	(20, 1, N'I didn''t even know you had a lawn mower', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 4, NULL),
	(21, 8, N'Well I did but I guess not anymore cause it''s gone', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 4, NULL),
	(22, 8, N'Lemme knwo if you happen to find it', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 4, NULL),

	(23, 9, N'👋', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 5, NULL),
	(24, 6, N'🍔❓', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 5, NULL),
	(25, 9, N'👍', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 5, NULL),
	(26, 6, N'👍', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 5, NULL);
	SET IDENTITY_INSERT dbo.[Messages] OFF;

	SET IDENTITY_INSERT dbo.MessageLikes ON;
	INSERT INTO dbo.MessageLikes
	(Id, LikingUserId, LikedMessageId, LikeDateTime)
	VALUES
	(1, 1, 3, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(2, 1, 5, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(3, 11, 5, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(4, 6, 9, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(5, 10, 9, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(6, 11, 9, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()));
	SET IDENTITY_INSERT dbo.MessageLikes OFF;

	SET IDENTITY_INSERT dbo.BlockedUsers ON;
	INSERT INTO dbo.BlockedUsers
	(Id, BlockingUserId, BlockedUserId, BlockDateTime)
	VALUES
	(1, 1, 13, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(2, 1, 9, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(3, 6, 10, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()));
	SET IDENTITY_INSERT dbo.BlockedUsers OFF;

	--NO IDENTITY
	INSERT INTO dbo.Profiles
	(UserId, [Description], ProfilePicture, ProfileVisibilityId)
	VALUES
	(1, NULL, NULL, 2),
	(2, 'My name is Hollie, don''t call me "Holly"', NULL, 1),
	(3, NULL, NULL, 1),
	(4, NULL, NULL, 1),
	(5, NULL, NULL, 1),
	(6, 'My name is Ricardo, feel free to add me 😊', NULL, 1),
	(7, 'Employed at [REDACTED]', NULL, 1),
	(8, NULL, NULL, 1),
	(9, NULL, NULL, 1),
	(10, NULL, NULL, 2),
	(11, NULL, NULL, 1),
	(12, 'Please don''t contact me unless I know you', NULL, 1),
	(13, 'Feel free to add me!!!', NULL, 1);

	SET IDENTITY_INSERT dbo.ProfileLikes ON;
	INSERT INTO dbo.ProfileLikes
	(Id, LikingUserId, LikedProfileId, LikeDateTime)
	VALUES
	(1, 1, 7, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(2, 1, 12, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(3, 6, 12, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(4, 12, 1, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(5, 12, 6, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(6, 6, 9, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(7, 9, 6, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(8, 4, 1, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(9, 9, 1, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(10, 8, 1, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
	(11, 1, 8, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()));
	SET IDENTITY_INSERT dbo.ProfileLikes OFF;
END;