using GymManagement.Contracts.Subscriptions;

namespace GymManagement.Api.Endpoints.Subscriptions;

public static class SubscriptionUtility
{
    public static SubscriptionType ToDto(Domain.Subscriptions.SubscriptionType subscriptionType)
    {
        return subscriptionType.Name switch
        {
            nameof(Domain.Subscriptions.SubscriptionType.Free) => SubscriptionType.Free,
            nameof(Domain.Subscriptions.SubscriptionType.Starter) => SubscriptionType.Starter,
            nameof(Domain.Subscriptions.SubscriptionType.Pro) => SubscriptionType.Pro,
            _ => throw new InvalidOperationException()
        };
    }
}