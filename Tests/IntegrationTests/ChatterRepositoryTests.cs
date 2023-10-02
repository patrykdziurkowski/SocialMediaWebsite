using Application;
using Application.Features.Chatter;
using Application.Features.Chatter.Interfaces;
using Dapper;
using FluentAssertions;
using System.Data;
using Tests.IntegrationTests.Fixtures;
using Xunit;

namespace Tests.IntegrationTests
{
    [Collection("DockerDatabaseCollection")]
    public class ChatterRepositoryTests
    {
        private readonly ChatterRepository _subject;
        private readonly IConnectionFactory _connectionFactory;

        public ChatterRepositoryTests(
            IntegrationTestApplicationFactory factory)
        {
            ChatterRepository? subject = (ChatterRepository?) factory.Services.GetService(typeof(IChatterRepository));
            IConnectionFactory? connectionFactory = (IConnectionFactory?) factory.Services.GetService(typeof(IConnectionFactory));

            if (subject is null || connectionFactory is null)
            {
                throw new ApplicationException("Test dependencies not found in DI container");
            }

            _subject = subject;
            _connectionFactory = connectionFactory;
        }

        [Fact]
        public async Task GetByIdAsync_GivenChatterId_ReturnsChatter()
        {
            //Arrange
            ChatterId id = new();
            await InsertFakeUserIntoDatabase(id);

            //Act
            Chatter chatter = await _subject.GetByIdAsync(id);

            //Assert
            chatter.Id.Should().Be(id);
        }


        private async Task InsertFakeUserIntoDatabase(ChatterId id)
        {
            using IDbConnection connection = _connectionFactory.GetConnection(ConnectionType.SqlConnection);
            connection.Open();

            await connection.ExecuteAsync(
                """
                INSERT INTO SocialMediaWebsite.dbo.Users
                (Id, UserName, Email, PasswordHash)
                VALUES
                (@Id, @UserName, @Email, @PasswordHash);
                """,
                new
                {
                    Id = id,
                    UserName = "JohnSmith123",
                    Email = "john@smith.com",
                    PasswordHash = "P@ssword1!"
                });
        }

    }
}
