using Application.Features.Chatter;
using Application.Features.Shared;
using System.Data;
using static Dapper.SqlMapper;

namespace Web
{
    public class ChatterIdTypeHandler : TypeHandler<ChatterId>
    {
        public override ChatterId Parse(object value)
        {
            Guid id = (Guid) value;
            return new ChatterId(id);
        }

        public override void SetValue(IDbDataParameter parameter, ChatterId value)
        {
            parameter.Value = value.Value;
            parameter.DbType = DbType.Guid;
        }
    }
}
