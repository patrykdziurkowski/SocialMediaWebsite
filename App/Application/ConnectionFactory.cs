using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public enum ConnectionType
    {
        SqlConnection
    }

    public class ConnectionFactory : IConnectionFactory
    {
        private readonly ConnectionStringProvider _connectionStringProvider;
        public ConnectionFactory(ConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }


        public IDbConnection GetConnection(ConnectionType connectionType)
        {
            string connectionString = _connectionStringProvider.GetConnectionString();

            if (connectionType == ConnectionType.SqlConnection)
            {
                return new SqlConnection(connectionString);
            }

            throw new NotImplementedException("No such connection type");
        }
    }
}
