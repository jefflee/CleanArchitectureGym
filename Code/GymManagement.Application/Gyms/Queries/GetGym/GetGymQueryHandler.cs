using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Gyms;
using MediatR;

namespace GymManagement.Application.Gyms.Queries.GetGym;

public class GetGymQueryHandler(IGymsRepository gymsRepository, ISubscriptionsRepository subscriptionsRepository)
    : IRequestHandler<GetGymQuery, ErrorOr<Gym>>
{
    public async Task<ErrorOr<Gym>> Handle(GetGymQuery request, CancellationToken cancellationToken)
    {
        if (!await subscriptionsRepository.ExistsAsync(request.SubscriptionId))
        {
            return Error.NotFound("Subscription not found");
        }

        if (await gymsRepository.GetByIdAsync(request.GymId) is not { } gym)
        {
            return Error.NotFound(description: "Gym not found");
        }

        return gym;
    }
}