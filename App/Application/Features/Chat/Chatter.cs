using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Chat
{
    public class Chatter
    {
        public Chatter(
            int id,
            string name,
            DateTimeOffset joinDateTime)
        {
            Id = id;
            Name = name;
            JoinDateTime = joinDateTime;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public DateTimeOffset JoinDateTime { get; private set; }
    }
}
