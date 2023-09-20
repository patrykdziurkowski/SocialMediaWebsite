﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Chat
{
    public class Chatter
    {
        private Chatter()
        {
            Id = default!;
            Name = default!;
            JoinDateTime = default!;
        }

        public Chatter(
            string name,
            DateTimeOffset joinDateTime)
        {
            Id = new ChatterId();
            Name = name;
            JoinDateTime = joinDateTime;
        }

        public ChatterId Id { get; private set; }
        public string Name { get; set; }
        public DateTimeOffset JoinDateTime { get; private set; }
    }
}
