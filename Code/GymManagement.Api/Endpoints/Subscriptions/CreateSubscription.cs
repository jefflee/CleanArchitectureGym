using GymManagement.Application.Subscriptions.Commands.CreateSubscription;
using GymManagement.Contracts.Subscriptions;
using MediatR;

namespace GymManagement.Api.Endpoints.Subscriptions
{
    public static class CreateSubscription
    {
        public static WebApplication AddCreatingSubscriptionEndpoint(this WebApplication app)
        {
            app.MapPost("/subscriptions", async (
                        CreateSubscriptionRequest request,
                        ISender mediator)
                    =>
                {
                    var command = new CreateSubscriptionCommand(
                        request.SubscriptionType.ToString(),
                        request.AdminId);

                    var createSubscriptionResult = await mediator.Send(command);

                    return createSubscriptionResult.MatchFirst(
                        subscription =>
                        {
                            var response = new SubscriptionResponse(subscription.Id, request.SubscriptionType);
                            return Results.Created($"/subscriptions/{subscription.Id}", response);
                        },
                        error => Results.Problem());
                })
                .WithName("CreateSubscription")
                .WithTags("Subscription")
                .WithOpenApi();

            return app;
        }
    }
}