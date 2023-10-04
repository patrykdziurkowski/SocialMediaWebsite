using Application.Features.Authentication;
using System.Data;
using static Dapper.SqlMapper;

namespace Web
{
    public class UserIdTypeHandler : TypeHandler<UserId>
    {
        public override UserId Parse(object value)
        {
            Guid id = (Guid) value;
            return new UserId(id);
        }

        public override void SetValue(IDbDataParameter parameter, UserId value)
        {
            parameter.Value = value.Value;
            parameter.DbType = DbType.Guid;
        }
    }
}
