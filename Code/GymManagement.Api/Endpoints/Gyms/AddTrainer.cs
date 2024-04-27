using GymManagement.Application.Gyms.AddTrainer;
using GymManagement.Contracts.Gyms;
using MediatR;

namespace GymManagement.Api.Endpoints.Gyms;

public static class AddTrainer
{
    public static WebApplication AddAddingTrainerEndpoint(this WebApplication app)
    {
        app.MapPost("/subscriptions/{subscriptionId}/gyms/{gymId}/trainers", async (
                    AddTrainerRequest request,
                    Guid subscriptionId,
                    Guid gymId,
                    ISender mediator)
                =>
            {
                var command = new AddTrainerCommand(subscriptionId, gymId, request.TrainerId);

                var addTrainerResult = await mediator.Send(command);

                return addTrainerResult.MatchFirst<IResult>(
                    success => Results.Ok(),
                    error => Results.Problem(error.Description));
            })
            .WithName("AddTrainer")
            .WithTags("Gym")
            .WithOpenApi();

        return app;
    }
}