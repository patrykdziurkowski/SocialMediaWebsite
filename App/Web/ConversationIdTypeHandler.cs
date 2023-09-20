using Application.Features.Chat;
using static Dapper.SqlMapper;
using System.Data;

namespace Web
{
    public class ConversationIdTypeHandler : TypeHandler<ConversationId>
    {
        public override ConversationId Parse(object value)
        {
            Guid id = (Guid) value;
            return new ConversationId(id);
        }

        public override void SetValue(IDbDataParameter parameter, ConversationId value)
        {
            parameter.Value = value.Value;
            parameter.DbType = DbType.Guid;
        }
    }
}
