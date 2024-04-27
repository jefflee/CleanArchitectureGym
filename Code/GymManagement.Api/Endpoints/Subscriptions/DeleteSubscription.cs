using GymManagement.Application.Subscriptions.Commands.DeleteSubscritpion;
using MediatR;

namespace GymManagement.Api.Endpoints.Subscriptions;

public static class DeleteSubscription
{
    public static WebApplication AddDeletingSubscriptionEndpoint(this WebApplication app)
    {
        app.MapDelete("/subscriptions/{subscriptionId}", async (
                    Guid subscriptionId,
                    ISender mediator)
                =>
            {
                var command = new DeleteSubscriptionCommand(subscriptionId);

                var deleteSubscriptionResult = await mediator.Send(command);


                return deleteSubscriptionResult.MatchFirst<IResult>(
                    _ => Results.NoContent(),
                    error => Results.Problem(error.Description));
            })
            .WithName("DeleteSubscription")
            .WithTags("Subscription")
            .WithOpenApi();

        return app;
    }
}