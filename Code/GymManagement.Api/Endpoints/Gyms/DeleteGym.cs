using GymManagement.Application.Gyms.DeleteGym;
using MediatR;

namespace GymManagement.Api.Endpoints.Gyms;

public static class DeleteGym
{
    public static WebApplication AddDeletingGymEndpoint(this WebApplication app)
    {
        app.MapDelete("/subscriptions/{subscriptionId}/gyms/{gymId}", async (
                    Guid subscriptionId,
                    Guid gymId,
                    ISender mediator)
                =>
            {
                var command = new DeleteGymCommand(subscriptionId, gymId);

                var deleteGymResult = await mediator.Send(command);

                return deleteGymResult.MatchFirst<IResult>(
                    _ => Results.NoContent(),
                    error => Results.Problem(error.Description));
            })
            .WithName("DeleteGym")
            .WithTags("Gym")
            .WithOpenApi();

        return app;
    }
}