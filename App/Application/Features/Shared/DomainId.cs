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
