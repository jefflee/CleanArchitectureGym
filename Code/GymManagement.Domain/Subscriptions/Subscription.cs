using ErrorOr;
using GymManagement.Domain.Gyms;
using Throw;

namespace GymManagement.Domain.Subscriptions
{
    public class Subscription
    {
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
        }

        public List<Gym> Gyms { get; set; } = new();

        public Guid Id { get; private set; }

        public SubscriptionType SubscriptionType { get; } = null!;

        public Guid AdminId { get; }

        public ErrorOr<Success> AddGym(Gym gym)
        {
            Gyms.Throw().IfTrue(p => p.Any(g => g.Id == gym.Id));

            if (Gyms.Count >= GetMaxGyms())
            {
                return SubscriptionErrors.CannotHaveMoreGymsThanSubscriptionAllows;
            }

            Gyms.Add(gym);

            return Result.Success;
        }

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

        public int GetMaxRooms()
        {
            return SubscriptionType.Name switch
            {
                nameof(SubscriptionType.Free) => 1,
                nameof(SubscriptionType.Starter) => 3,
                nameof(SubscriptionType.Pro) => int.MaxValue,
                _ => throw new InvalidOperationException()
            };
        }

        public int GetMaxDailySessions()
        {
            return SubscriptionType.Name switch
            {
                nameof(SubscriptionType.Free) => 4,
                nameof(SubscriptionType.Starter) => int.MaxValue,
                nameof(SubscriptionType.Pro) => int.MaxValue,
                _ => throw new InvalidOperationException()
            };
        }

        public bool HasGym(Guid gymId)
        {
            return Gyms.Any(gym => gym.Id == gymId);
        }

        public void RemoveGym(Guid gymId)
        {
            Gyms.Throw().IfFalse(p => p.Any(gym => gym.Id == gymId));

            // Remove element that gym.Id == gymId from Gyms list
            Gyms.RemoveAll(gym => gym.Id == gymId);
        }
    }
}