using GymManagement.Application.Gyms.Commands.CreateGym;
using GymManagement.Contracts.Gyms;
using MediatR;

namespace GymManagement.Api.Endpoints.Gyms;

public static class CreateGym
{
    public static WebApplication AddCreatingGymEndpoint(this WebApplication app)
    {
        app.MapPost("/subscriptions/{subscriptionId}/gyms", async (
                    CreateGymRequest request,
                    Guid subscriptionId,
                    ISender mediator)
                =>
            {
                var command = new CreateGymCommand(request.Name, subscriptionId);

                var createGymResult = await mediator.Send(command);

                return createGymResult.MatchFirst(
                    gym => Results.Created(
                        $"/subscriptions/{subscriptionId}/gyms/{gym.Id}",
                        new GymResponse(gym.Id, gym.Name)),
                    error => Results.Problem(error.Description));
            })
            .WithName("CreateGym")
            .WithTags("Gym")
            .WithOpenApi();

        return app;
    }
}