using Application.Features.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Chat
{
    public class ConversationId : DomainId
    {
        public ConversationId(Guid value) : base(value)
        {
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
