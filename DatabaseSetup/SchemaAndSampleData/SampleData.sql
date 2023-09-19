IF (NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Tests')) 
BEGIN
    EXEC ('CREATE SCHEMA Tests AUTHORIZATION [dbo]')
END

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
	
	BEGIN TRANSACTION SampleDataInsertTransaction;

	BEGIN TRY
		DELETE FROM SocialMediaWebsite.dbo.MessageLikes;
		DELETE FROM SocialMediaWebsite.dbo.[Messages];
		DELETE FROM SocialMediaWebsite.dbo.ConversationUsers;
		DELETE FROM SocialMediaWebsite.dbo.Conversations;
		DELETE FROM SocialMediaWebsite.dbo.CommentLikes;
		DELETE FROM SocialMediaWebsite.dbo.Comments;
		DELETE FROM SocialMediaWebsite.dbo.PostLikes;
		DELETE FROM SocialMediaWebsite.dbo.Posts;
		DELETE FROM SocialMediaWebsite.dbo.UserGroupMemberships;
		DELETE FROM SocialMediaWebsite.dbo.GroupInvitations;
		DELETE FROM SocialMediaWebsite.dbo.GroupApplications;
		DELETE FROM SocialMediaWebsite.dbo.Groups;
		DELETE FROM SocialMediaWebsite.dbo.GroupJoinRestrictions;
		DELETE FROM SocialMediaWebsite.dbo.ProfileLikes;
		DELETE FROM SocialMediaWebsite.dbo.Profiles;
		DELETE FROM SocialMediaWebsite.dbo.Visibility;
		DELETE FROM SocialMediaWebsite.dbo.BlockedUsers;
		DELETE FROM SocialMediaWebsite.dbo.Users;
	
	

		DECLARE @AppStartDate DATETIMEOFFSET(7) = '2018-06-17 11:25:39';

		INSERT INTO SocialMediaWebsite.dbo.Visibility
		(Id, Visibility)
		VALUES
		(1, 'Public'),
		(2, 'Hidden');


		INSERT INTO SocialMediaWebsite.dbo.Users 
		(Id, UserName, FirstName, LastName, Email, IsEmailConfirmed, PasswordHash, JoinDateTime)
		VALUES
		(CONVERT(uniqueidentifier, 'a5d6e79a-899c-4809-beb3-cf1da37ba4a1'), 'JohnSmith', 'John', 'Smith', 'jsmith@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '12b7f7e1-f927-4b27-9bc0-89f1574e61aa'), 'HollieJames123', 'Hollie', 'James', 'hollieJ@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, 'ee1bcd99-b373-4d11-8fb4-155993c56e5c'), 'Cargigs', 'Carmen', 'Rigs', 'criggs@someemail.net', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, 'b846e523-353c-4d64-9eb3-0b8780a7d9a6'), 'ZakiHainz', 'Zaki', 'Haines', 'hainzZ@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, 'a2b5085b-3f70-41d0-9d8f-7bebf2f29589'), 'RichardP', 'Richard', 'Pollard', 'rpollard@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, 'ba549fea-fe7b-421e-81a5-fca5e0001d31'), 'RicardooO', 'Ricardo', 'Horton', 'ricardoHorton@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '79c1a21b-d945-4fc1-a9aa-be13a4ae608e'), 'AngelaElliott423', 'Angela', 'Elliott', 'angelaElliott@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, 'a65f46cc-7af2-484d-b368-4422b6d0a323'), 'JoeSmith', 'Joe', 'Smith', 'joesmith@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '6c7aa137-ab5a-4071-96bc-2b9b36f1dcc3'), 'Grant9423', 'Ashwin', 'Grant', 'agrant123@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '9d1ef319-62d6-4eb5-be8b-37c1eec5899c'), 'anonymousDude123', NULL, NULL, 'anonymousDude@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, 'a1a56c53-a6d2-4812-9d0f-a288c055f0a8'), 'AleenaM', NULL, 'Morrow', 'aleena.morrow@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, 'd382431d-3c19-42f6-ae97-85a2864065fc'), 'JohnSmith1', 'John', 'Smith', 'otherJsmith@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, 'deec83a4-46d4-4b45-8ec3-83dbc73a4cd1'), 'ValentinaDillon1', NULL, NULL, 'valentina.1@email.com', CONVERT(BIT, ROUND(1 * Tests.GetRand(), 0)), Tests.GenerateSampleFakePasswordHash(), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()));



		INSERT INTO SocialMediaWebsite.dbo.Conversations
		(Id, Title, [Description], OwnerUserId, CreationDateTime)
		VALUES
		(CONVERT(uniqueidentifier, 'd577e688-2cce-47b3-a782-c91cc87890e6'), 'WorkConversation', 'This is our conversation meant for work purposes only', CONVERT(uniqueidentifier, 'a5d6e79a-899c-4809-beb3-cf1da37ba4a1'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, 'd779b0df-2d61-45d2-85cc-3fe465dd71dc'), 'FunConvo', NULL, CONVERT(uniqueidentifier, 'ba549fea-fe7b-421e-81a5-fca5e0001d31'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '7bef2ebf-7ad0-4457-a7bf-7ce16f6c5288'), 'GroupConversation1', NULL, CONVERT(uniqueidentifier, 'ba549fea-fe7b-421e-81a5-fca5e0001d31'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, 'a0a34c3d-239e-4c88-927a-a25e5be94ec5'), 'FamilyConversation', 'Family@', CONVERT(uniqueidentifier, 'a65f46cc-7af2-484d-b368-4422b6d0a323'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '3978dc79-0f3f-40b6-9ddc-782c28ab6c5d'), 'GroupConversation1', NULL, CONVERT(uniqueidentifier, '6c7aa137-ab5a-4071-96bc-2b9b36f1dcc3'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()));



		INSERT INTO SocialMediaWebsite.dbo.ConversationUsers
		(Id, ConversationId, UserId, ConversationIsHidden)
		VALUES
		(CONVERT(uniqueidentifier, 'fea07d5d-c10a-4fd0-8bd8-1a02d4b927fb'), CONVERT(uniqueidentifier, 'd577e688-2cce-47b3-a782-c91cc87890e6'), CONVERT(uniqueidentifier, 'a5d6e79a-899c-4809-beb3-cf1da37ba4a1'), 0),
		(CONVERT(uniqueidentifier, 'd857ba78-93bd-45a5-8f05-a8e293d6206b'), CONVERT(uniqueidentifier, 'd577e688-2cce-47b3-a782-c91cc87890e6'), CONVERT(uniqueidentifier, '79c1a21b-d945-4fc1-a9aa-be13a4ae608e'), 0),
		(CONVERT(uniqueidentifier, '186a4b4a-3f61-4705-9f6a-7eacac65fe96'), CONVERT(uniqueidentifier, 'd577e688-2cce-47b3-a782-c91cc87890e6'), CONVERT(uniqueidentifier, 'a1a56c53-a6d2-4812-9d0f-a288c055f0a8'), 0),
		(CONVERT(uniqueidentifier, '08b498fe-afe2-4bda-b5c7-c4b4c3d90902'), CONVERT(uniqueidentifier, 'd577e688-2cce-47b3-a782-c91cc87890e6'), CONVERT(uniqueidentifier, 'd382431d-3c19-42f6-ae97-85a2864065fc'), 0),

		(CONVERT(uniqueidentifier, 'f1f1b2b9-f9f6-460e-93dc-77acf97004b3'), CONVERT(uniqueidentifier, 'd779b0df-2d61-45d2-85cc-3fe465dd71dc'), CONVERT(uniqueidentifier, 'deec83a4-46d4-4b45-8ec3-83dbc73a4cd1'), 0),
		(CONVERT(uniqueidentifier, 'a5e651f5-d581-4389-8e5a-d99a2cdcc9bb'), CONVERT(uniqueidentifier, 'd779b0df-2d61-45d2-85cc-3fe465dd71dc'), CONVERT(uniqueidentifier, 'd382431d-3c19-42f6-ae97-85a2864065fc'), 0),
		(CONVERT(uniqueidentifier, '9c781a36-9542-4e13-83ea-d5dde977f90f'), CONVERT(uniqueidentifier, 'd779b0df-2d61-45d2-85cc-3fe465dd71dc'), CONVERT(uniqueidentifier, 'a1a56c53-a6d2-4812-9d0f-a288c055f0a8'), 0),
		(CONVERT(uniqueidentifier, 'ebe0f480-8103-46aa-ba3b-cb2c8390d9a9'), CONVERT(uniqueidentifier, 'd779b0df-2d61-45d2-85cc-3fe465dd71dc'), CONVERT(uniqueidentifier, '9d1ef319-62d6-4eb5-be8b-37c1eec5899c'), 0),
		(CONVERT(uniqueidentifier, '9dc80005-4137-46c5-94a7-fdd3f695531c'), CONVERT(uniqueidentifier, 'd779b0df-2d61-45d2-85cc-3fe465dd71dc'), CONVERT(uniqueidentifier, 'ba549fea-fe7b-421e-81a5-fca5e0001d31'), 0),

		(CONVERT(uniqueidentifier, 'be99a0a1-b75f-4a05-a35c-6b2a0a13498f'), CONVERT(uniqueidentifier, '7bef2ebf-7ad0-4457-a7bf-7ce16f6c5288'), CONVERT(uniqueidentifier, 'ba549fea-fe7b-421e-81a5-fca5e0001d31'), 0),
		(CONVERT(uniqueidentifier, '3866455a-b985-4f46-8911-8e9308022157'), CONVERT(uniqueidentifier, '7bef2ebf-7ad0-4457-a7bf-7ce16f6c5288'), CONVERT(uniqueidentifier, 'd382431d-3c19-42f6-ae97-85a2864065fc'), 0),

		(CONVERT(uniqueidentifier, '0746990a-8b82-42ba-8c9d-d5e2698484b4'), CONVERT(uniqueidentifier, 'a0a34c3d-239e-4c88-927a-a25e5be94ec5'), CONVERT(uniqueidentifier, 'a5d6e79a-899c-4809-beb3-cf1da37ba4a1'), 1),
		(CONVERT(uniqueidentifier, '4bc6d03d-ece6-487f-80b7-43570b3a6033'), CONVERT(uniqueidentifier, 'a0a34c3d-239e-4c88-927a-a25e5be94ec5'), CONVERT(uniqueidentifier, 'a65f46cc-7af2-484d-b368-4422b6d0a323'), 0),

		(CONVERT(uniqueidentifier, '9abeca19-2b33-4438-b20d-7bd894fc55f6'), CONVERT(uniqueidentifier, '3978dc79-0f3f-40b6-9ddc-782c28ab6c5d'), CONVERT(uniqueidentifier, 'ba549fea-fe7b-421e-81a5-fca5e0001d31'), 0),
		(CONVERT(uniqueidentifier, 'e67ea4b7-fa2e-4383-92ea-e5a09528995c'), CONVERT(uniqueidentifier, '3978dc79-0f3f-40b6-9ddc-782c28ab6c5d'), CONVERT(uniqueidentifier, '6c7aa137-ab5a-4071-96bc-2b9b36f1dcc3'), 0);



		INSERT INTO SocialMediaWebsite.dbo.[Messages]
		(Id, AuthorUserId, [Text], MessageDateTime, Conversationid, ReplyMessageId)
		VALUES
		(CONVERT(uniqueidentifier, '4f55fa19-5811-4b68-a803-e2190cfb8e6a'), CONVERT(uniqueidentifier, 'a5d6e79a-899c-4809-beb3-cf1da37ba4a1'), N'Hello everyone!', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'd577e688-2cce-47b3-a782-c91cc87890e6'), NULL),
		(CONVERT(uniqueidentifier, '838f2f6c-1313-43aa-b610-9efaaa03d4a1'), CONVERT(uniqueidentifier, 'a5d6e79a-899c-4809-beb3-cf1da37ba4a1'), N'Let me know once everyone is here', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'd577e688-2cce-47b3-a782-c91cc87890e6'), CONVERT(uniqueidentifier, '4f55fa19-5811-4b68-a803-e2190cfb8e6a')),
		(CONVERT(uniqueidentifier, 'e5a02bbc-79f0-4f4b-92e0-5c3be8ec2692'), CONVERT(uniqueidentifier, '79c1a21b-d945-4fc1-a9aa-be13a4ae608e'), N'Hi John I''m here! 👋', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'd577e688-2cce-47b3-a782-c91cc87890e6'), CONVERT(uniqueidentifier, '838f2f6c-1313-43aa-b610-9efaaa03d4a1')),
		(CONVERT(uniqueidentifier, '19bbb249-6be1-426d-9ea7-51a5fbd6abce'), CONVERT(uniqueidentifier, 'a1a56c53-a6d2-4812-9d0f-a288c055f0a8'), N'Howdy', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'd577e688-2cce-47b3-a782-c91cc87890e6'), NULL),
		(CONVERT(uniqueidentifier, 'c49577cb-84a0-4aaa-9d3a-7a4799fc0f3e'), CONVERT(uniqueidentifier, 'd382431d-3c19-42f6-ae97-85a2864065fc'), N'When are we getting started?', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'd577e688-2cce-47b3-a782-c91cc87890e6'), CONVERT(uniqueidentifier, '4f55fa19-5811-4b68-a803-e2190cfb8e6a')),

		(CONVERT(uniqueidentifier, '5b3acc6b-5db4-4297-8e61-45b77f4a7a4a'), CONVERT(uniqueidentifier, 'deec83a4-46d4-4b45-8ec3-83dbc73a4cd1'), N'does this thing work', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'd779b0df-2d61-45d2-85cc-3fe465dd71dc'), NULL),
		(CONVERT(uniqueidentifier, '789376ae-bf9f-449c-8f35-245760a76c3c'), CONVERT(uniqueidentifier, 'deec83a4-46d4-4b45-8ec3-83dbc73a4cd1'), N'hello anybody there? 🥱', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'd779b0df-2d61-45d2-85cc-3fe465dd71dc'), NULL),
		(CONVERT(uniqueidentifier, '6ed66b9d-559d-4f0d-82d1-1da4dce7625a'), CONVERT(uniqueidentifier, 'deec83a4-46d4-4b45-8ec3-83dbc73a4cd1'), N'my internet must be out...', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'd779b0df-2d61-45d2-85cc-3fe465dd71dc'), NULL),
		(CONVERT(uniqueidentifier, '051be569-6e11-4b20-9e68-5e8d44d56a29'), CONVERT(uniqueidentifier, 'd382431d-3c19-42f6-ae97-85a2864065fc'), N'Stop spamming', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'd779b0df-2d61-45d2-85cc-3fe465dd71dc'), CONVERT(uniqueidentifier, '6ed66b9d-559d-4f0d-82d1-1da4dce7625a')),

		(CONVERT(uniqueidentifier, '4916cb6f-2605-4ba6-82a4-f282f889b392'), CONVERT(uniqueidentifier, 'ba549fea-fe7b-421e-81a5-fca5e0001d31'), N'Yo', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, '7bef2ebf-7ad0-4457-a7bf-7ce16f6c5288'), NULL),
		(CONVERT(uniqueidentifier, '0a468437-9373-4e82-a31a-301d21f6547e'), CONVERT(uniqueidentifier, 'ba549fea-fe7b-421e-81a5-fca5e0001d31'), N'Wanna come to the party', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, '7bef2ebf-7ad0-4457-a7bf-7ce16f6c5288'), NULL),
		(CONVERT(uniqueidentifier, 'd310ae1d-d2e1-4e11-b1ca-11fe8f28c798'), CONVERT(uniqueidentifier, 'd382431d-3c19-42f6-ae97-85a2864065fc'), N'I don''t know man', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, '7bef2ebf-7ad0-4457-a7bf-7ce16f6c5288'), NULL),
		(CONVERT(uniqueidentifier, '858297fb-7778-4ae6-932a-1e090a614dcf'), CONVERT(uniqueidentifier, 'ba549fea-fe7b-421e-81a5-fca5e0001d31'), N'its gonna be fun', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, '7bef2ebf-7ad0-4457-a7bf-7ce16f6c5288'), NULL),
		(CONVERT(uniqueidentifier, '95ab2a16-aefa-4d83-97e6-df15ec036851'), CONVERT(uniqueidentifier, 'd382431d-3c19-42f6-ae97-85a2864065fc'), N'ok', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, '7bef2ebf-7ad0-4457-a7bf-7ce16f6c5288'), NULL),
		(CONVERT(uniqueidentifier, '7ead9903-9638-43d3-bbc0-6c0f38e10211'), CONVERT(uniqueidentifier, 'd382431d-3c19-42f6-ae97-85a2864065fc'), N' ', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, '7bef2ebf-7ad0-4457-a7bf-7ce16f6c5288'), NULL),

		(CONVERT(uniqueidentifier, '85e42451-2328-4f5d-82ad-42721f10532c'), CONVERT(uniqueidentifier, 'a65f46cc-7af2-484d-b368-4422b6d0a323'), N'Hi John how''s work', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'a0a34c3d-239e-4c88-927a-a25e5be94ec5'), NULL),
		(CONVERT(uniqueidentifier, '3f4b18e7-3883-4a55-a585-a544238058a5'), CONVERT(uniqueidentifier, 'a5d6e79a-899c-4809-beb3-cf1da37ba4a1'), N'Why it is going very well, now please do not bother me', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'a0a34c3d-239e-4c88-927a-a25e5be94ec5'), NULL),
		(CONVERT(uniqueidentifier, '7b0efaaf-969f-476a-aaa8-f2508b03a8c4'), CONVERT(uniqueidentifier, 'a65f46cc-7af2-484d-b368-4422b6d0a323'), N'Did you see my lawn mower I can''t fidn it', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'a0a34c3d-239e-4c88-927a-a25e5be94ec5'), NULL),
		(CONVERT(uniqueidentifier, 'b46117a5-7286-4bc5-8636-fa727e9015f5'), CONVERT(uniqueidentifier, 'a65f46cc-7af2-484d-b368-4422b6d0a323'), N'I need to mow the grass', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'a0a34c3d-239e-4c88-927a-a25e5be94ec5'), NULL),
		(CONVERT(uniqueidentifier, 'd9d1d311-81fa-41bf-a66d-a58b4a6528f8'), CONVERT(uniqueidentifier, 'a5d6e79a-899c-4809-beb3-cf1da37ba4a1'), N'I didn''t even know you had a lawn mower', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'a0a34c3d-239e-4c88-927a-a25e5be94ec5'), NULL),
		(CONVERT(uniqueidentifier, '8fcbf515-81ee-4aec-95cf-3e8371628540'), CONVERT(uniqueidentifier, 'a65f46cc-7af2-484d-b368-4422b6d0a323'), N'Well I did but I guess not anymore cause it''s gone', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'a0a34c3d-239e-4c88-927a-a25e5be94ec5'), NULL),
		(CONVERT(uniqueidentifier, '05b28eb4-30de-40b7-94ad-1c0c236e39f1'), CONVERT(uniqueidentifier, 'a65f46cc-7af2-484d-b368-4422b6d0a323'), N'Lemme knwo if you happen to find it', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'a0a34c3d-239e-4c88-927a-a25e5be94ec5'), NULL),

		(CONVERT(uniqueidentifier, '8d088c48-5fa4-47b6-94fb-72d3c5b0d9a3'), CONVERT(uniqueidentifier, '6c7aa137-ab5a-4071-96bc-2b9b36f1dcc3'), N'👋', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, '3978dc79-0f3f-40b6-9ddc-782c28ab6c5d'), NULL),
		(CONVERT(uniqueidentifier, '34b46f50-0a34-4ea2-afd5-b7909322fdfc'), CONVERT(uniqueidentifier, 'ba549fea-fe7b-421e-81a5-fca5e0001d31'), N'🍔❓', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, '3978dc79-0f3f-40b6-9ddc-782c28ab6c5d'), NULL),
		(CONVERT(uniqueidentifier, '3ce40757-10a1-4815-99ca-40d3934ae25d'), CONVERT(uniqueidentifier, '6c7aa137-ab5a-4071-96bc-2b9b36f1dcc3'), N'👍', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, '3978dc79-0f3f-40b6-9ddc-782c28ab6c5d'), NULL),
		(CONVERT(uniqueidentifier, '95f7a4cf-8de2-4bbc-bd27-b5b3db99e3e4'), CONVERT(uniqueidentifier, 'ba549fea-fe7b-421e-81a5-fca5e0001d31'), N'👍', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, '3978dc79-0f3f-40b6-9ddc-782c28ab6c5d'), NULL);



		INSERT INTO SocialMediaWebsite.dbo.MessageLikes
		(Id, LikingUserId, LikedMessageId, LikeDateTime)
		VALUES
		(CONVERT(uniqueidentifier, '17217694-32fd-45de-bb7e-52e67d4f8689'), CONVERT(uniqueidentifier, 'a5d6e79a-899c-4809-beb3-cf1da37ba4a1'), CONVERT(uniqueidentifier, 'e5a02bbc-79f0-4f4b-92e0-5c3be8ec2692'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, 'd922aa32-0097-45ec-ab01-3a5644869aee'), CONVERT(uniqueidentifier, 'a5d6e79a-899c-4809-beb3-cf1da37ba4a1'), CONVERT(uniqueidentifier, 'c49577cb-84a0-4aaa-9d3a-7a4799fc0f3e'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '23d1d498-302d-4a33-9e61-78010c43d83b'), CONVERT(uniqueidentifier, 'a1a56c53-a6d2-4812-9d0f-a288c055f0a8'), CONVERT(uniqueidentifier, 'c49577cb-84a0-4aaa-9d3a-7a4799fc0f3e'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, 'ec92cb2f-c622-4e03-b2d7-08a2676c6366'), CONVERT(uniqueidentifier, 'ba549fea-fe7b-421e-81a5-fca5e0001d31'), CONVERT(uniqueidentifier, '051be569-6e11-4b20-9e68-5e8d44d56a29'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '31e0f3c8-ac51-450b-a209-61b679027509'), CONVERT(uniqueidentifier, '9d1ef319-62d6-4eb5-be8b-37c1eec5899c'), CONVERT(uniqueidentifier, '051be569-6e11-4b20-9e68-5e8d44d56a29'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '2eb76052-a79d-4528-95b5-0ad1bf619d05'), CONVERT(uniqueidentifier, 'a1a56c53-a6d2-4812-9d0f-a288c055f0a8'), CONVERT(uniqueidentifier, '051be569-6e11-4b20-9e68-5e8d44d56a29'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()));



		INSERT INTO SocialMediaWebsite.dbo.BlockedUsers
		(Id, BlockingUserId, BlockedUserId, BlockDateTime)
		VALUES
		(CONVERT(uniqueidentifier, '50ee704b-e94a-41df-be96-f5fd8106c4fe'), CONVERT(uniqueidentifier, 'a5d6e79a-899c-4809-beb3-cf1da37ba4a1'), CONVERT(uniqueidentifier, 'deec83a4-46d4-4b45-8ec3-83dbc73a4cd1'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '07165c88-fc27-4dde-a9ad-9e8f786245d0'), CONVERT(uniqueidentifier, 'a5d6e79a-899c-4809-beb3-cf1da37ba4a1'), CONVERT(uniqueidentifier, '6c7aa137-ab5a-4071-96bc-2b9b36f1dcc3'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '2107f2e6-72b2-4035-a3f8-ba32c46b2aa9'), CONVERT(uniqueidentifier, 'ba549fea-fe7b-421e-81a5-fca5e0001d31'), CONVERT(uniqueidentifier, '9d1ef319-62d6-4eb5-be8b-37c1eec5899c'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()));


		INSERT INTO SocialMediaWebsite.dbo.Profiles
		(UserId, [Description], ProfilePicture, ProfileVisibilityId)
		VALUES
		(CONVERT(uniqueidentifier, 'a5d6e79a-899c-4809-beb3-cf1da37ba4a1'), NULL, NULL, 2),
		(CONVERT(uniqueidentifier, '12b7f7e1-f927-4b27-9bc0-89f1574e61aa'), N'My name is Hollie, don''t call me "Holly"', NULL, 1),
		(CONVERT(uniqueidentifier, 'ee1bcd99-b373-4d11-8fb4-155993c56e5c'), NULL, NULL, 1),
		(CONVERT(uniqueidentifier, 'b846e523-353c-4d64-9eb3-0b8780a7d9a6'), NULL, NULL, 1),
		(CONVERT(uniqueidentifier, 'a2b5085b-3f70-41d0-9d8f-7bebf2f29589'), NULL, NULL, 1),
		(CONVERT(uniqueidentifier, 'ba549fea-fe7b-421e-81a5-fca5e0001d31'), N'My name is Ricardo, feel free to add me 😊', NULL, 1),
		(CONVERT(uniqueidentifier, '79c1a21b-d945-4fc1-a9aa-be13a4ae608e'), N'Employed at [REDACTED]', NULL, 1),
		(CONVERT(uniqueidentifier, 'a65f46cc-7af2-484d-b368-4422b6d0a323'), NULL, NULL, 1),
		(CONVERT(uniqueidentifier, '6c7aa137-ab5a-4071-96bc-2b9b36f1dcc3'), NULL, NULL, 1),
		(CONVERT(uniqueidentifier, '9d1ef319-62d6-4eb5-be8b-37c1eec5899c'), NULL, NULL, 2),
		(CONVERT(uniqueidentifier, 'a1a56c53-a6d2-4812-9d0f-a288c055f0a8'), NULL, NULL, 1),
		(CONVERT(uniqueidentifier, 'd382431d-3c19-42f6-ae97-85a2864065fc'), N'Please don''t contact me unless I know you', NULL, 1),
		(CONVERT(uniqueidentifier, 'deec83a4-46d4-4b45-8ec3-83dbc73a4cd1'), N'Feel free to add me!!!', NULL, 1);


		INSERT INTO SocialMediaWebsite.dbo.ProfileLikes
		(Id, LikingUserId, LikedProfileId, LikeDateTime)
		VALUES
		(CONVERT(uniqueidentifier, 'd4fadb67-c995-486d-9137-371d35fe983e'), CONVERT(uniqueidentifier, 'a5d6e79a-899c-4809-beb3-cf1da37ba4a1'), CONVERT(uniqueidentifier, '79c1a21b-d945-4fc1-a9aa-be13a4ae608e'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, 'fe5915dc-1f12-4618-9d1c-84e2ecdc113a'), CONVERT(uniqueidentifier, 'a5d6e79a-899c-4809-beb3-cf1da37ba4a1'), CONVERT(uniqueidentifier, 'd382431d-3c19-42f6-ae97-85a2864065fc'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '8cad5b50-de9e-421e-97f6-7306a23d9ede'), CONVERT(uniqueidentifier, 'ba549fea-fe7b-421e-81a5-fca5e0001d31'), CONVERT(uniqueidentifier, 'd382431d-3c19-42f6-ae97-85a2864065fc'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '9b3c4215-f746-47c0-9e83-df311951c7f8'), CONVERT(uniqueidentifier, 'd382431d-3c19-42f6-ae97-85a2864065fc'), CONVERT(uniqueidentifier, 'a5d6e79a-899c-4809-beb3-cf1da37ba4a1'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '6b1017fc-a3c6-4feb-a1e9-f23f2dbc31b9'), CONVERT(uniqueidentifier, 'd382431d-3c19-42f6-ae97-85a2864065fc'), CONVERT(uniqueidentifier, 'ba549fea-fe7b-421e-81a5-fca5e0001d31'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, 'a1014827-bc32-4531-8475-b08ce368b42a'), CONVERT(uniqueidentifier, 'ba549fea-fe7b-421e-81a5-fca5e0001d31'), CONVERT(uniqueidentifier, '6c7aa137-ab5a-4071-96bc-2b9b36f1dcc3'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '0046cf7c-4426-4a4e-bc0f-e2b485a13c0d'), CONVERT(uniqueidentifier, '6c7aa137-ab5a-4071-96bc-2b9b36f1dcc3'), CONVERT(uniqueidentifier, 'ba549fea-fe7b-421e-81a5-fca5e0001d31'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '970780de-d175-4699-bb79-972982f890cd'), CONVERT(uniqueidentifier, 'b846e523-353c-4d64-9eb3-0b8780a7d9a6'), CONVERT(uniqueidentifier, 'a5d6e79a-899c-4809-beb3-cf1da37ba4a1'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '248311b3-7492-4421-b1e8-616df1fba86c'), CONVERT(uniqueidentifier, '6c7aa137-ab5a-4071-96bc-2b9b36f1dcc3'), CONVERT(uniqueidentifier, 'a5d6e79a-899c-4809-beb3-cf1da37ba4a1'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '4026978f-864c-4fb8-9cac-6aa12bf6e670'), CONVERT(uniqueidentifier, 'a65f46cc-7af2-484d-b368-4422b6d0a323'), CONVERT(uniqueidentifier, 'a5d6e79a-899c-4809-beb3-cf1da37ba4a1'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, 'dd04f31b-fe63-452e-ba25-9db41cc08a9e'), CONVERT(uniqueidentifier, 'a5d6e79a-899c-4809-beb3-cf1da37ba4a1'), CONVERT(uniqueidentifier, 'a65f46cc-7af2-484d-b368-4422b6d0a323'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()));



		INSERT INTO SocialMediaWebsite.dbo.GroupJoinRestrictions
		(Id, Restriction)
		VALUES
		(1, N'None'),
		(2, N'InviteOnly'),
		(3, N'LinkOnly'),
		(4, N'ApplyOnly');



		INSERT INTO SocialMediaWebsite.dbo.Groups
		(Id, [Name], [Description], VisibilityId, JoinRestrictionId, IsActive, CreationDateTime)
		VALUES
		(CONVERT(uniqueidentifier, '52e5661c-3881-4a54-b519-018fb41e8952'), N'Book Discussion', NULL, 1, 2, 1, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '26ce86cd-f8f8-4469-a057-09c66dfb4c34'), N'Local Marketplace', NULL, 1, 4, 1, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '2412deec-6a66-4f60-a58c-f6f8f3d00402'), N'Some Group', '', 2, 3, 1, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '6db94ab0-e5a5-49e8-89e4-2da766b10c62'), N'[REDACTED] Organization', NULL, 2, 2, 1, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '20b40d39-d793-4b2e-9334-d14d6195839b'), N'Whatever Discussion', NULL, 1, 1, 0, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()));



		INSERT INTO SocialMediaWebsite.dbo.GroupInvitations
		(Id, InvitingUserId, InvitedUserId, GroupId, [Message], InviteDateTime, IsActive)
		VALUES
		(CONVERT(uniqueidentifier, 'e147819f-1554-4337-b426-053b4b0da0c5'), CONVERT(uniqueidentifier, 'ba549fea-fe7b-421e-81a5-fca5e0001d31'), CONVERT(uniqueidentifier, 'd382431d-3c19-42f6-ae97-85a2864065fc'), CONVERT(uniqueidentifier, '52e5661c-3881-4a54-b519-018fb41e8952'), NULL, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 0),
		(CONVERT(uniqueidentifier, 'f66f28bc-57a4-447a-b3e9-c5b232a79365'), CONVERT(uniqueidentifier, 'ba549fea-fe7b-421e-81a5-fca5e0001d31'), CONVERT(uniqueidentifier, 'd382431d-3c19-42f6-ae97-85a2864065fc'), CONVERT(uniqueidentifier, '52e5661c-3881-4a54-b519-018fb41e8952'), NULL, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 0),
		(CONVERT(uniqueidentifier, 'a840064c-a263-4421-9aeb-82291d17a751'), CONVERT(uniqueidentifier, 'ba549fea-fe7b-421e-81a5-fca5e0001d31'), CONVERT(uniqueidentifier, 'd382431d-3c19-42f6-ae97-85a2864065fc'), CONVERT(uniqueidentifier, '52e5661c-3881-4a54-b519-018fb41e8952'), NULL, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 1),
		(CONVERT(uniqueidentifier, '32a71f82-1349-4df1-b47c-642a788bc95f'), CONVERT(uniqueidentifier, 'a5d6e79a-899c-4809-beb3-cf1da37ba4a1'), CONVERT(uniqueidentifier, 'a1a56c53-a6d2-4812-9d0f-a288c055f0a8'), CONVERT(uniqueidentifier, '6db94ab0-e5a5-49e8-89e4-2da766b10c62'), NULL, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 1),
		(CONVERT(uniqueidentifier, 'bc3d05a2-ac2b-4f8e-9a7e-b570c977011e'), CONVERT(uniqueidentifier, 'a5d6e79a-899c-4809-beb3-cf1da37ba4a1'), CONVERT(uniqueidentifier, '79c1a21b-d945-4fc1-a9aa-be13a4ae608e'), CONVERT(uniqueidentifier, '6db94ab0-e5a5-49e8-89e4-2da766b10c62'), NULL, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 1);



		INSERT INTO SocialMediaWebsite.dbo.GroupApplications
		(Id, ApplyingUserId, GroupId, [Message], AppliedDateTime)
		VALUES
		(CONVERT(uniqueidentifier, '328bfc2f-2625-4821-ae68-eb0bf6f4f55b'), CONVERT(uniqueidentifier, 'ba549fea-fe7b-421e-81a5-fca5e0001d31'), CONVERT(uniqueidentifier, '26ce86cd-f8f8-4469-a057-09c66dfb4c34'), NULL, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '39162e0b-abd8-4987-8c70-54f830b5a57d'), CONVERT(uniqueidentifier, 'b846e523-353c-4d64-9eb3-0b8780a7d9a6'), CONVERT(uniqueidentifier, '26ce86cd-f8f8-4469-a057-09c66dfb4c34'), N'i wish to sell my washing machine', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '727f1100-4997-41d8-8460-ecfad5b0362c'), CONVERT(uniqueidentifier, 'ee1bcd99-b373-4d11-8fb4-155993c56e5c'), CONVERT(uniqueidentifier, '26ce86cd-f8f8-4469-a057-09c66dfb4c34'), NULL, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()));



		INSERT INTO SocialMediaWebsite.dbo.UserGroupMemberships
		(Id, UserId, GroupId, JoinDateTime, WasInvited)
		VALUES
		(CONVERT(uniqueidentifier, '02f46328-0160-4f0e-bb0d-ae1c4421e9d8'), CONVERT(uniqueidentifier, 'b846e523-353c-4d64-9eb3-0b8780a7d9a6'), CONVERT(uniqueidentifier, '52e5661c-3881-4a54-b519-018fb41e8952'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 1),
		(CONVERT(uniqueidentifier, '88cd0a94-7715-4f13-993a-d1f1999d494f'), CONVERT(uniqueidentifier, 'ee1bcd99-b373-4d11-8fb4-155993c56e5c'), CONVERT(uniqueidentifier, '52e5661c-3881-4a54-b519-018fb41e8952'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 1),
		(CONVERT(uniqueidentifier, '1664135b-e36e-4f58-ac4f-3412610a98ec'), CONVERT(uniqueidentifier, 'a65f46cc-7af2-484d-b368-4422b6d0a323'), CONVERT(uniqueidentifier, '52e5661c-3881-4a54-b519-018fb41e8952'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 1),
	
		(CONVERT(uniqueidentifier, '9999e2fa-af09-476a-b4ae-8f49c6d708bf'), CONVERT(uniqueidentifier, 'a5d6e79a-899c-4809-beb3-cf1da37ba4a1'), CONVERT(uniqueidentifier, '6db94ab0-e5a5-49e8-89e4-2da766b10c62'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 1),
		(CONVERT(uniqueidentifier, 'd054ced7-da67-4ade-9ee6-1a0af3e9d01a'), CONVERT(uniqueidentifier, '79c1a21b-d945-4fc1-a9aa-be13a4ae608e'), CONVERT(uniqueidentifier, '6db94ab0-e5a5-49e8-89e4-2da766b10c62'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 1),
		(CONVERT(uniqueidentifier, '119d9d0c-6f2b-48be-8ec9-44ccddeccc17'), CONVERT(uniqueidentifier, 'a1a56c53-a6d2-4812-9d0f-a288c055f0a8'), CONVERT(uniqueidentifier, '6db94ab0-e5a5-49e8-89e4-2da766b10c62'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 1),
		(CONVERT(uniqueidentifier, '39a629ab-6979-45b7-91f4-6b6c17fbd09c'), CONVERT(uniqueidentifier, 'd382431d-3c19-42f6-ae97-85a2864065fc'), CONVERT(uniqueidentifier, '6db94ab0-e5a5-49e8-89e4-2da766b10c62'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 1),
	
		(CONVERT(uniqueidentifier, '9bd2b8d7-4bee-4bcd-a2d7-1a633ba222c5'), CONVERT(uniqueidentifier, 'a65f46cc-7af2-484d-b368-4422b6d0a323'), CONVERT(uniqueidentifier, '26ce86cd-f8f8-4469-a057-09c66dfb4c34'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 0),
		(CONVERT(uniqueidentifier, 'fe5b54ce-9595-4247-9075-d77a3408d58a'), CONVERT(uniqueidentifier, 'a2b5085b-3f70-41d0-9d8f-7bebf2f29589'), CONVERT(uniqueidentifier, '26ce86cd-f8f8-4469-a057-09c66dfb4c34'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 0),
		
		(CONVERT(uniqueidentifier, '3458804d-2101-483d-83ba-a2b5dbe08083'), CONVERT(uniqueidentifier, 'ba549fea-fe7b-421e-81a5-fca5e0001d31'), CONVERT(uniqueidentifier, '2412deec-6a66-4f60-a58c-f6f8f3d00402'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 0),
		(CONVERT(uniqueidentifier, '7f931a04-de0f-4ecb-9bdc-7b4d61bf1259'), CONVERT(uniqueidentifier, 'd382431d-3c19-42f6-ae97-85a2864065fc'), CONVERT(uniqueidentifier, '2412deec-6a66-4f60-a58c-f6f8f3d00402'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 0),
		(CONVERT(uniqueidentifier, 'fc85b79e-99ad-4b25-bb51-1eb8dcfa40bf'), CONVERT(uniqueidentifier, '12b7f7e1-f927-4b27-9bc0-89f1574e61aa'), CONVERT(uniqueidentifier, '2412deec-6a66-4f60-a58c-f6f8f3d00402'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 0),
		(CONVERT(uniqueidentifier, '05c79a53-3247-4043-89a5-13441ad2a494'), CONVERT(uniqueidentifier, 'a65f46cc-7af2-484d-b368-4422b6d0a323'), CONVERT(uniqueidentifier, '2412deec-6a66-4f60-a58c-f6f8f3d00402'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 1),
		(CONVERT(uniqueidentifier, '58fe3b91-98be-4d18-a032-fc2201786be2'), CONVERT(uniqueidentifier, '6c7aa137-ab5a-4071-96bc-2b9b36f1dcc3'), CONVERT(uniqueidentifier, '2412deec-6a66-4f60-a58c-f6f8f3d00402'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 0),

		(CONVERT(uniqueidentifier, '0bb4eeb7-1845-48d6-84e7-a1ec1b0a97f0'), CONVERT(uniqueidentifier, 'a2b5085b-3f70-41d0-9d8f-7bebf2f29589'), CONVERT(uniqueidentifier, '20b40d39-d793-4b2e-9334-d14d6195839b'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), 0);


	

		INSERT INTO SocialMediaWebsite.dbo.Posts
		(Id, Title, [Description], PostDateTime, AuthorUserId, GroupId, IsCommentingEnabled, SharedPostId)
		VALUES
		(CONVERT(uniqueidentifier, '9335e8fd-be05-485f-b84e-1cdf068e5125'), N'Great day to work', N'Wowie, today is truly a great day to be working! Like this post if you agree!', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'a5d6e79a-899c-4809-beb3-cf1da37ba4a1'), CONVERT(uniqueidentifier, '6db94ab0-e5a5-49e8-89e4-2da766b10c62'), 1, NULL),
		(CONVERT(uniqueidentifier, 'b06a9a2e-62ff-436c-8597-4854ee67ad77'), N'How''s your day', NULL, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'a65f46cc-7af2-484d-b368-4422b6d0a323'), NULL, 0, NULL),
		(CONVERT(uniqueidentifier, '387a3a68-0736-4d19-b544-2fb3a180762a'), N'Mistake', N'You forgot to enable the comments! 💬💬💬❗❗', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, '12b7f7e1-f927-4b27-9bc0-89f1574e61aa'), NULL, 1, CONVERT(uniqueidentifier, 'b06a9a2e-62ff-436c-8597-4854ee67ad77')),
		(CONVERT(uniqueidentifier, 'ff20404e-ab7c-4475-81a8-4ad317093bb7'), N'Is anyone selling', N'selling a hacksaw??', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'a65f46cc-7af2-484d-b368-4422b6d0a323'), CONVERT(uniqueidentifier, '26ce86cd-f8f8-4469-a057-09c66dfb4c34'), 1, NULL),
		(CONVERT(uniqueidentifier, '98cfeedf-8ea8-4e60-b7e3-2a214f2135c2'), N'Lawn mower', N'I wish to buy a new lawn mower I got $100 to make it happen send me a message', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'a65f46cc-7af2-484d-b368-4422b6d0a323'), CONVERT(uniqueidentifier, '26ce86cd-f8f8-4469-a057-09c66dfb4c34'), 1, NULL),
		(CONVERT(uniqueidentifier, '736c9ff8-4fae-48de-9dfc-7151d042b8f2'), N'❗❗❗Used furtniture❗❗❗', N'Greetings, I wish to sell a lot of used furniture, please send me a message if you''re interested.', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'a2b5085b-3f70-41d0-9d8f-7bebf2f29589'), CONVERT(uniqueidentifier, '26ce86cd-f8f8-4469-a057-09c66dfb4c34'), 1, NULL),
		(CONVERT(uniqueidentifier, '47db477b-f6ff-4291-ba30-992e960f1aab'), N'Group rules:📝', N'Will fill out in the future', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'a2b5085b-3f70-41d0-9d8f-7bebf2f29589'), CONVERT(uniqueidentifier, '20b40d39-d793-4b2e-9334-d14d6195839b'), 0, NULL),
		(CONVERT(uniqueidentifier, '3152acda-0824-4ca2-a3e7-0d17b6e4a9f1'), N'did u see the news?', N'i saw the news', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, '9d1ef319-62d6-4eb5-be8b-37c1eec5899c'), NULL, 1, NULL),
		(CONVERT(uniqueidentifier, '9c1ed966-6e52-4fd8-8414-312d49dfb0fa'), N'Some people are just clueless', N'Thoughts?', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, '12b7f7e1-f927-4b27-9bc0-89f1574e61aa'), NULL, 1, NULL),
		(CONVERT(uniqueidentifier, 'bb2a6754-f6e6-4d8f-855a-99c236aa3b6f'), N'Let''s create a chain', N'of quote posts', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'a1a56c53-a6d2-4812-9d0f-a288c055f0a8'), CONVERT(uniqueidentifier, '2412deec-6a66-4f60-a58c-f6f8f3d00402'), 1, NULL),
		(CONVERT(uniqueidentifier, '80275d92-6e49-4546-96a5-c58a1793380d'), N'adaasdasvcxklxcvvzxcxcvxcvm,xcvz1234#', NULL, Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'd382431d-3c19-42f6-ae97-85a2864065fc'), CONVERT(uniqueidentifier, '2412deec-6a66-4f60-a58c-f6f8f3d00402'), 1, CONVERT(uniqueidentifier, '80275d92-6e49-4546-96a5-c58a1793380d')),
		(CONVERT(uniqueidentifier, '197007d3-aac0-4098-85ed-43e9497db06b'), N'Stop spamming', N'Or I''m going to kick you out', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'deec83a4-46d4-4b45-8ec3-83dbc73a4cd1'), CONVERT(uniqueidentifier, '2412deec-6a66-4f60-a58c-f6f8f3d00402'), 1, CONVERT(uniqueidentifier, '197007d3-aac0-4098-85ed-43e9497db06b')),
		(CONVERT(uniqueidentifier, '9a12c489-8935-45d9-af0d-f3dbc314cf6e'), N'Events', N'Is anything planned for tomorrow?', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'ee1bcd99-b373-4d11-8fb4-155993c56e5c'), CONVERT(uniqueidentifier, '52e5661c-3881-4a54-b519-018fb41e8952'), 1, NULL),
		(CONVERT(uniqueidentifier, '73213502-1be8-4b86-b87c-0a7c3534675f'), N'Invitation to SomeGroup', N'Could someone please invite me to SomeGroup?', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'ba549fea-fe7b-421e-81a5-fca5e0001d31'), NULL, 1, NULL),
		(CONVERT(uniqueidentifier, '056d1e5e-9705-4023-89eb-e2cdc3de1db6'), N'Boring', N'Man this platform is dead...', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, '9d1ef319-62d6-4eb5-be8b-37c1eec5899c'), NULL, 1, NULL),
		(CONVERT(uniqueidentifier, 'ac3c442c-dd0a-4104-8a42-f302361a7e76'), N'any1 wanna talk cars', 'i love cars', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'a65f46cc-7af2-484d-b368-4422b6d0a323'), NULL, 1, NULL);

	

		INSERT INTO SocialMediaWebsite.dbo.PostLikes
		(Id, LikingUserId, LikedPostId, LikeDateTime)
		VALUES
		(CONVERT(uniqueidentifier, '8a2cc460-ed29-4cd7-966d-970b441a4358'), CONVERT(uniqueidentifier, 'a5d6e79a-899c-4809-beb3-cf1da37ba4a1'), CONVERT(uniqueidentifier, '197007d3-aac0-4098-85ed-43e9497db06b'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '66422a5d-b181-4e77-8363-82a102c73f0d'), CONVERT(uniqueidentifier, 'ba549fea-fe7b-421e-81a5-fca5e0001d31'), CONVERT(uniqueidentifier, '197007d3-aac0-4098-85ed-43e9497db06b'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, 'c5fbe056-5b9b-4eec-9c87-38604595e7b6'), CONVERT(uniqueidentifier, '9d1ef319-62d6-4eb5-be8b-37c1eec5899c'), CONVERT(uniqueidentifier, '197007d3-aac0-4098-85ed-43e9497db06b'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '058c8880-f31a-4947-93cd-97d654c28a99'), CONVERT(uniqueidentifier, 'd382431d-3c19-42f6-ae97-85a2864065fc'), CONVERT(uniqueidentifier, 'bb2a6754-f6e6-4d8f-855a-99c236aa3b6f'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, 'a97cb57f-cd45-40e2-89ec-da30f0f21229'), CONVERT(uniqueidentifier, 'a1a56c53-a6d2-4812-9d0f-a288c055f0a8'), CONVERT(uniqueidentifier, 'bb2a6754-f6e6-4d8f-855a-99c236aa3b6f'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, 'b20b89c3-b5bf-40a9-baa6-3e58d5fddb86'), CONVERT(uniqueidentifier, 'a1a56c53-a6d2-4812-9d0f-a288c055f0a8'), CONVERT(uniqueidentifier, '80275d92-6e49-4546-96a5-c58a1793380d'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, 'b2e4d394-9b15-4b09-9d4c-f49dc29fb9b1'), CONVERT(uniqueidentifier, 'a65f46cc-7af2-484d-b368-4422b6d0a323'), CONVERT(uniqueidentifier, '387a3a68-0736-4d19-b544-2fb3a180762a'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '615db303-9866-48a9-8590-3062ffbedc16'), CONVERT(uniqueidentifier, '12b7f7e1-f927-4b27-9bc0-89f1574e61aa'), CONVERT(uniqueidentifier, 'ff20404e-ab7c-4475-81a8-4ad317093bb7'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '867a656c-236a-43af-99ff-2d2d1bc6f5f6'), CONVERT(uniqueidentifier, 'a2b5085b-3f70-41d0-9d8f-7bebf2f29589'), CONVERT(uniqueidentifier, '9c1ed966-6e52-4fd8-8414-312d49dfb0fa'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()));



		INSERT INTO SocialMediaWebsite.dbo.Comments
		(Id, [Text], CommentDateTime, ParentPostId, ParentCommentId, AuthorUserId)
		VALUES
		(CONVERT(uniqueidentifier, '9393a09c-0d64-4859-975d-f36beef60286'), N'Agreed ma''am!', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, '9c1ed966-6e52-4fd8-8414-312d49dfb0fa'), NULL, CONVERT(uniqueidentifier, 'a65f46cc-7af2-484d-b368-4422b6d0a323')),
		(CONVERT(uniqueidentifier, 'ced7d1cf-8b7d-419f-be22-c8260282dad4'), N'i think she was talking about you dude', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, '9c1ed966-6e52-4fd8-8414-312d49dfb0fa'), CONVERT(uniqueidentifier, '9393a09c-0d64-4859-975d-f36beef60286'), CONVERT(uniqueidentifier, '9d1ef319-62d6-4eb5-be8b-37c1eec5899c')),
		(CONVERT(uniqueidentifier, '30f7ae13-0e3b-4b46-83d9-dbbea4075e9c'), N'Oh!', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, '9c1ed966-6e52-4fd8-8414-312d49dfb0fa'), CONVERT(uniqueidentifier, 'ced7d1cf-8b7d-419f-be22-c8260282dad4'), CONVERT(uniqueidentifier, 'a65f46cc-7af2-484d-b368-4422b6d0a323')),
		(CONVERT(uniqueidentifier, '2fa47f87-03d9-43ea-a3b7-06d79041ff82'), N'omg so true!', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, '9c1ed966-6e52-4fd8-8414-312d49dfb0fa'), NULL, CONVERT(uniqueidentifier, 'deec83a4-46d4-4b45-8ec3-83dbc73a4cd1')),
		(CONVERT(uniqueidentifier, '2c92c105-9e8d-48a5-82ce-90bc8e0efd31'), N'Sorry', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, '197007d3-aac0-4098-85ed-43e9497db06b'), NULL, CONVERT(uniqueidentifier, 'd382431d-3c19-42f6-ae97-85a2864065fc')),
		(CONVERT(uniqueidentifier, 'cb479bae-f103-4106-91dc-fafaf0b34b03'), N'not that I know of 😂', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, '9a12c489-8935-45d9-af0d-f3dbc314cf6e'), NULL, CONVERT(uniqueidentifier, 'b846e523-353c-4d64-9eb3-0b8780a7d9a6')),
		(CONVERT(uniqueidentifier, 'f5b84c45-552a-4a40-a87b-58ce469ee930'), N'Why I would love to!', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'ac3c442c-dd0a-4104-8a42-f302361a7e76'), NULL, CONVERT(uniqueidentifier, 'a2b5085b-3f70-41d0-9d8f-7bebf2f29589')),
		(CONVERT(uniqueidentifier, 'e86c80d3-f95a-4c83-b814-d04bf57d5877'), N'Wrong post, apologies.', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, 'ac3c442c-dd0a-4104-8a42-f302361a7e76'), NULL, CONVERT(uniqueidentifier, 'a2b5085b-3f70-41d0-9d8f-7bebf2f29589')),
		(CONVERT(uniqueidentifier, 'b896f2f8-d9b4-4de1-adbf-d4b7eb8349f4'), N'Agreed', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, '056d1e5e-9705-4023-89eb-e2cdc3de1db6'), NULL, CONVERT(uniqueidentifier, 'd382431d-3c19-42f6-ae97-85a2864065fc')),
		(CONVERT(uniqueidentifier, 'd37bdd13-7082-4533-8a86-0d8a569298fc'), N'Quit being so negative', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, '056d1e5e-9705-4023-89eb-e2cdc3de1db6'), NULL, CONVERT(uniqueidentifier, 'a5d6e79a-899c-4809-beb3-cf1da37ba4a1')),
		(CONVERT(uniqueidentifier, '35f139a4-5819-4e3c-af96-af87aa3d668d'), N'Heck yes, brother!', Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()), CONVERT(uniqueidentifier, '3152acda-0824-4ca2-a3e7-0d17b6e4a9f1'), NULL, CONVERT(uniqueidentifier, 'a65f46cc-7af2-484d-b368-4422b6d0a323'));



		INSERT INTO SocialMediaWebsite.dbo.CommentLikes
		(Id, LikingUserId, LikedCommentId, LikeDateTime)
		VALUES
		(CONVERT(uniqueidentifier, 'eea70873-fdac-417a-87a0-a3459479b268'), CONVERT(uniqueidentifier, '12b7f7e1-f927-4b27-9bc0-89f1574e61aa'), CONVERT(uniqueidentifier, 'ced7d1cf-8b7d-419f-be22-c8260282dad4'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, 'b085a85c-1d50-4200-b38f-f4acb086d0cb'), CONVERT(uniqueidentifier, '9d1ef319-62d6-4eb5-be8b-37c1eec5899c'), CONVERT(uniqueidentifier, '30f7ae13-0e3b-4b46-83d9-dbbea4075e9c'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, 'db464f5a-ddae-4c9b-9b9d-b6d01523ea80'), CONVERT(uniqueidentifier, 'a2b5085b-3f70-41d0-9d8f-7bebf2f29589'), CONVERT(uniqueidentifier, '30f7ae13-0e3b-4b46-83d9-dbbea4075e9c'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '39984b58-75c7-491d-b07f-71f593fa4ba3'), CONVERT(uniqueidentifier, 'd382431d-3c19-42f6-ae97-85a2864065fc'), CONVERT(uniqueidentifier, '30f7ae13-0e3b-4b46-83d9-dbbea4075e9c'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '3af3bc43-c3d1-4140-b177-af001060c052'), CONVERT(uniqueidentifier, 'a5d6e79a-899c-4809-beb3-cf1da37ba4a1'), CONVERT(uniqueidentifier, '30f7ae13-0e3b-4b46-83d9-dbbea4075e9c'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, '6de1cb13-8314-4fa9-8d9e-52a9523304f3'), CONVERT(uniqueidentifier, 'deec83a4-46d4-4b45-8ec3-83dbc73a4cd1'), CONVERT(uniqueidentifier, '2c92c105-9e8d-48a5-82ce-90bc8e0efd31'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE())),
		(CONVERT(uniqueidentifier, 'bd5b918c-19e0-4413-9693-0d1a4cd16254'), CONVERT(uniqueidentifier, '12b7f7e1-f927-4b27-9bc0-89f1574e61aa'), CONVERT(uniqueidentifier, 'b896f2f8-d9b4-4de1-adbf-d4b7eb8349f4'), Tests.GenerateSampleFakeDate(@AppStartDate, GETDATE()));


		COMMIT TRANSACTION SchemaSetupTransaction;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION SampleDataInsertTransaction;
		PRINT 'Inserting sample data failed. Transaction rolled back.';
		THROW;
	END CATCH;
END;
GO
Tests.InsertSampleData;