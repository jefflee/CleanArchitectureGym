using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using MediatR;

namespace GymManagement.Application.Gyms.DeleteGym;

public class DeleteGymCommandHandler(ISubscriptionsRepository subscriptionsRepository,
        IGymsRepository gymsRepository,
        IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteGymCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(DeleteGymCommand command, CancellationToken cancellationToken)
    {
        var gym = await gymsRepository.GetByIdAsync(command.GymId);

        if (gym is null)
        {
            return Error.NotFound(description: "Gym not found");
        }

        var subscription = await subscriptionsRepository.GetByIdAsync(command.SubscriptionId);

        if (subscription is null)
        {
            return Error.NotFound(description: "Subscription not found");
        }

        if (!subscription.HasGym(command.GymId))
        {
            return Error.Unexpected(description: "The subscription doesn't contain the Gym");
        }

        subscription.RemoveGym(command.GymId);

        await subscriptionsRepository.UpdateAsync(subscription);
        await gymsRepository.RemoveGymAsync(gym);
        await unitOfWork.CommitChangesAsync();

        return Result.Deleted;
    }
}