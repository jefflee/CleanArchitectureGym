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

                    var subscriptionId = await mediator.Send(command);

                    var response = new SubscriptionResponse(subscriptionId, request.SubscriptionType);

                    return Results.Created($"/subscriptions/{subscriptionId}", response);
                })
                .WithName("CreateASubscription")
                .WithTags("Subscription")
                .WithOpenApi();

            return app;
        }
    }
}