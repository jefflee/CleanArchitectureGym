using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Subscriptions;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.CreateSubscription;

public class CreateSubscriptionCommandHandler(ISubscriptionsRepository subscriptionsRepository, IUnitOfWork unitOfWork,
        IAdminsRepository adminsRepository)
    : IRequestHandler<CreateSubscriptionCommand, ErrorOr<Subscription>>
{
    public async Task<ErrorOr<Subscription>> Handle(CreateSubscriptionCommand request,
        CancellationToken cancellationToken)
    {
        var admin = await adminsRepository.GetByIdAsync(request.AdminId);

        if (admin is null)
        {
            return Error.NotFound(description: "Admin not found");
        }

        // Create a subscription
        var subscription = new Subscription(
            request.SubscriptionType,
            request.AdminId);

        if (admin.SubscriptionId is not null)
        {
            return Error.Conflict(description: "Admin already has an active subscription");
        }

        admin.SetSubscription(subscription);

        // Add it to the database
        await subscriptionsRepository.AddSubscriptionAsync(subscription);
        await adminsRepository.UpdateAsync(admin);
        await unitOfWork.CommitChangesAsync();

        // Return subscription
        return subscription;
    }
}