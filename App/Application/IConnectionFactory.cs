using System.Data;

namespace Application
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection(Type type);
    }
}