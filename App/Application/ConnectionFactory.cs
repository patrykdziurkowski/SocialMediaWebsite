using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;
        public ConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public IDbConnection GetConnection(ConnectionType connectionType)
        {
            string connectionStringName = _configuration["ConnectionStringName"]
                ?? throw new ApplicationException("ConnectionStringName not found in configuration.");
            string connectionString = _configuration.GetConnectionString(connectionStringName)
                ?? throw new ApplicationException("ConnectionString not found in configuration. Enter a ConnectionString into appsettings.json or secrets.json");

            if (connectionType == ConnectionType.SqlConnection)
            {
                return new SqlConnection(connectionString);
            }

            throw new NotImplementedException("No such connection type");
        }
    }
}
