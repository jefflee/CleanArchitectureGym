using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Rooms;
using MediatR;

namespace GymManagement.Application.Rooms.Commands.CreateRoom;

public class CreateRoomCommandHandler(ISubscriptionsRepository subscriptionsRepository,
        IGymsRepository gymsRepository,
        IUnitOfWork unitOfWork)
    : IRequestHandler<CreateRoomCommand, ErrorOr<Room>>
{
    public async Task<ErrorOr<Room>> Handle(CreateRoomCommand command, CancellationToken cancellationToken)
    {
        var gym = await gymsRepository.GetByIdAsync(command.GymId);

        if (gym is null)
        {
            return Error.NotFound(description: "Gym not found");
        }

        var subscription = await subscriptionsRepository.GetByIdAsync(gym.SubscriptionId);

        if (subscription is null)
        {
            return Error.Unexpected(description: "Subscription not found");
        }

        var room = new Room(
            command.RoomName,
            gym.Id,
            subscription.GetMaxDailySessions());

        var addGymResult = gym.AddRoom(room);

        if (addGymResult.IsError)
        {
            return addGymResult.Errors;
        }

        // Note: the room itself isn't stored in our database, but rather
        // in the "SessionManagement" system that is not in scope of this course.
        await gymsRepository.UpdateGymAsync(gym);
        await unitOfWork.CommitChangesAsync();

        return room;
    }
}