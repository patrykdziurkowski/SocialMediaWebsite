using Application.Features.Authentication.Interfaces;
using Dapper;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authentication
{
    public class UserRepository : IUserRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        public UserRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Result> Register(User user)
        {
            using IDbConnection connection = _connectionFactory.GetConnection(ConnectionType.SqlConnection);
            connection.Open();

            int numberOfAffectedRows = await connection.ExecuteAsync(
                """
                INSERT INTO Users
                (UserName, Email, PasswordHash)
                VALUES
                (@UserName, @Email, @PasswordHash)
                """,
                new
                {
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
