using Application.Features.Shared;

namespace Application.Features.Chatter
{
    public class Chatter : AggreggateRoot
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
