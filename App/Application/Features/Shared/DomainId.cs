using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Shared
{
    public abstract class DomainId : ValueObject
    {
        public DomainId(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; init; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
