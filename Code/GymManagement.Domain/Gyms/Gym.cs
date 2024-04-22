using ErrorOr;
using GymManagement.Domain.Rooms;
using Throw;

namespace GymManagement.Domain.Gyms;

public class Gym
{
    // _maxRooms is a private field that is only set in the constructor and never changed.
    // Its value is customer decided. In this project, it is in CreateRoomCommandHandler.
    // The interesting thins is that this value is from Subscription model.
    // Ths subscriptionId is also from the other model.
    // I think creating a duplicated property is not a good idea, but the domain can't access external data store.
    // So, it is a trade-off.
    private readonly int _maxRooms;
    private readonly List<Guid> _roomIds = new();
    private readonly List<Guid> _trainerIds = new();

    public Gym(
        string name,
        int maxRooms,
        Guid subscriptionId,
        Guid? id = null)
    {
        Name = name;
        _maxRooms = maxRooms;
        SubscriptionId = subscriptionId;
        Id = id ?? Guid.NewGuid();
    }

    private Gym()
    {
    }

    public Guid Id { get; }

    public string Name { get; init; } = null!;
    public Guid SubscriptionId { get; init; }

    public ErrorOr<Success> AddRoom(Room room)
    {
        _roomIds.Throw().IfContains(room.Id);

        if (_roomIds.Count >= _maxRooms)
        {
            return GymErrors.CannotHaveMoreRoomsThanSubscriptionAllows;
        }

        _roomIds.Add(room.Id);

        return Result.Success;
    }

    public bool HasRoom(Guid roomId)
    {
        return _roomIds.Contains(roomId);
    }

    public ErrorOr<Success> AddTrainer(Guid trainerId)
    {
        if (_trainerIds.Contains(trainerId))
        {
            return Error.Conflict(description: "Trainer already added to gym");
        }

        _trainerIds.Add(trainerId);
        return Result.Success;
    }

    public bool HasTrainer(Guid trainerId)
    {
        return _trainerIds.Contains(trainerId);
    }

    public void RemoveRoom(Guid roomId)
    {
        _roomIds.Remove(roomId);
    }
}