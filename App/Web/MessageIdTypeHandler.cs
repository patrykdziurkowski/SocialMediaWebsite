using Application.Features.Chat;
using static Dapper.SqlMapper;
using System.Data;

namespace Web
{
    public class MessageIdTypeHandler : TypeHandler<MessageId>
    {
        public override MessageId Parse(object value)
        {
            Guid id = (Guid) value;
            return new MessageId(id);
        }

        public override void SetValue(IDbDataParameter parameter, MessageId value)
        {
            parameter.Value = value.Value;
            parameter.DbType = DbType.Guid;
        }
    }
}
