namespace GymManagement.Domain.Subscriptions
{
    public class Subscription
    {
        private readonly List<Guid> _gymIds = new();

        private readonly int _maxGyms;

        private Subscription()
        {
        }

        public Subscription(
            SubscriptionType subscriptionType,
            Guid adminId,
            Guid? id = null)
        {
            SubscriptionType = subscriptionType;
            AdminId = adminId;
            Id = id ?? Guid.NewGuid();

            _maxGyms = GetMaxGyms();
        }

        public Guid Id { get; private set; }
        public SubscriptionType SubscriptionType { get; } = null!;

        public Guid AdminId { get; }

        public int GetMaxGyms()
        {
            return SubscriptionType.Name switch
            {
                nameof(SubscriptionType.Free) => 1,
                nameof(SubscriptionType.Starter) => 1,
                nameof(SubscriptionType.Pro) => 3,
                _ => throw new InvalidOperationException()
            };
        }
    }
}