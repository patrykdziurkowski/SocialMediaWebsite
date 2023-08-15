using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class ConnectionFactory : IConnectionFactory
    {
        private ConnectionStringProvider _connectionStringProvider;
        public ConnectionFactory(ConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public IDbConnection GetConnection(Type type)
        {
            string connectionString = _connectionStringProvider.GetConnectionString();

            if (type == typeof(SqlConnection))
            {
                return new SqlConnection(connectionString);
            }

            throw new NotImplementedException("No such connection type");
        }
    }
}
