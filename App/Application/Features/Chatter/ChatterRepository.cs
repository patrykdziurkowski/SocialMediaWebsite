using Application.Features.Chatter.Interfaces;
using Dapper;
using System.Data;

namespace Application.Features.Chatter
{
    public class ChatterRepository : IChatterRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public ChatterRepository(
            IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Chatter> GetByIdAsync(ChatterId chatterId)
        {
            using IDbConnection connection = _connectionFactory.GetConnection(ConnectionType.SqlConnection);
            connection.Open();

            return await connection.QuerySingleAsync<Chatter>(
                $"""
                SELECT
                    Id AS {nameof(Chatter.Id)},
                    UserName AS {nameof(Chatter.Name)},
                    JoinDateTime AS {nameof(Chatter.JoinDateTime)}
                FROM Users
                WHERE Id = @ChatterId
                """,
                new { ChatterId = chatterId });
        }

    }
}
