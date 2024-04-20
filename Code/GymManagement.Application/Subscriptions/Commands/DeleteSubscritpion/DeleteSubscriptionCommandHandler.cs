using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.DeleteSubscritpion;

public class DeleteSubscriptionCommandHandler(IAdminsRepository adminsRepository,
    ISubscriptionsRepository subscriptionsRepository,
    IUnitOfWork unitOfWork,
    IGymsRepository gymsRepository) : IRequestHandler<DeleteSubscriptionCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> Handle(DeleteSubscriptionCommand command, CancellationToken cancellationToken)
    {
        var subscription = await subscriptionsRepository.GetByIdAsync(command.SubscriptionId);

        if (subscription is null)
        {
            return Error.NotFound(description: "Subscription not found");
        }

        var admin = await adminsRepository.GetByIdAsync(subscription.AdminId);

        if (admin is null)
        {
            return Error.Unexpected(description: "Admin not found");
        }

        admin.DeleteSubscription(command.SubscriptionId);

        var gymsToDelete = await gymsRepository.ListBySubscriptionIdAsync(command.SubscriptionId);

        await adminsRepository.UpdateAsync(admin);
        await subscriptionsRepository.RemoveSubscriptionAsync(subscription);
        await gymsRepository.RemoveRangeAsync(gymsToDelete);
        await unitOfWork.CommitChangesAsync();

        return Result.Deleted;
    }
}