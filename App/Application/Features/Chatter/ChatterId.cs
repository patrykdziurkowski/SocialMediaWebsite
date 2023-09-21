using Application.Features.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Chatter
{
    public class ChatterId : DomainId
    {
        public ChatterId() : base(Guid.NewGuid())
        {
        }

        public ChatterId(Guid value) : base(value)
        {
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
