using GymManagement.Application.Gyms.Queries.ListGyms;
using GymManagement.Contracts.Gyms;
using MediatR;

namespace GymManagement.Api.Endpoints.Gyms;

public static class ListGyms
{
    public static WebApplication AddListingGymEndpoint(this WebApplication app)
    {
        app.MapGet("/subscriptions/{subscriptionId}/gyms", async (
                    Guid subscriptionId,
                    ISender mediator)
                =>
            {
                var command = new ListGymsQuery(subscriptionId);

                var listGymsResult = await mediator.Send(command);

                return listGymsResult.MatchFirst(
                    gyms => Results.Ok(gyms.ConvertAll(gym => new GymResponse(gym.Id, gym.Name))),
                    error => Results.Problem(error.Description));
            })
            .WithName("ListGyms")
            .WithTags("Gym")
            .WithOpenApi();

        return app;
    }
}