using Application.Features.Authentication.Interfaces;
using Dapper;
using FluentResults;
using System.Data;

namespace Application.Features.Authentication
{
    public class UserRepository : IUserRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        public UserRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Result<User>> GetUserByUserNameAsync(string userName)
        {
            using IDbConnection connection = _connectionFactory.GetConnection(ConnectionType.SqlConnection);
            connection.Open();

            User? user = await connection.QueryFirstOrDefaultAsync<User>(
                $"""
                SELECT
                    Id AS {nameof(User.Id)},
                    UserName AS {nameof(User.UserName)},
                    Email AS {nameof(User.Email)},
                    PasswordHash AS {nameof(User.PasswordHash)}
                FROM Users
                WHERE UserName = @UserName
                """,
                new { UserName = userName });

            if (user is null)
            {
                return Result.Fail("No user with such password and username combination was found.");
            }

            return Result.Ok(user);
        }

        public async Task<Result> Register(User user)
        {
            using IDbConnection connection = _connectionFactory.GetConnection(ConnectionType.SqlConnection);
            connection.Open();

            int numberOfAffectedRows = await connection.ExecuteAsync(
                """
                INSERT INTO Users
                (Id, UserName, Email, PasswordHash)
                VALUES
                (@Id, @UserName, @Email, @PasswordHash)
                """,
                new
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PasswordHash = user.PasswordHash
                });

            if (numberOfAffectedRows == 1)
            {
                return Result.Ok();
            }
            return Result.Fail($"{numberOfAffectedRows} rows affected");
        }

    }
}
