using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Shared
{
    public interface IEventHandler
    {
        Task Handle(
            DomainEvent domainEvent,
            IDbConnection connection,
            IDbTransaction transaction);
    }
}
