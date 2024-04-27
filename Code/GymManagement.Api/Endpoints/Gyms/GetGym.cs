using GymManagement.Application.Gyms.Queries.GetGym;
using GymManagement.Contracts.Gyms;
using MediatR;

namespace GymManagement.Api.Endpoints.Gyms;

public static class GetGym
{
    public static WebApplication AddGettingGymEndpoint(this WebApplication app)
    {
        app.MapGet("/subscriptions/{subscriptionId}/gyms/{gymId}", async (
                    Guid subscriptionId, Guid gymId,
                    ISender mediator)
                =>
            {
                var command = new GetGymQuery(subscriptionId, gymId);

                var getGymResult = await mediator.Send(command);

                return getGymResult.MatchFirst(
                    gym => Results.Ok(new GymResponse(gym.Id, gym.Name)),
                    error => Results.Problem(error.Description));
            })
            .WithName("GetGym")
            .WithTags("Gym")
            .WithOpenApi();

        return app;
    }
}