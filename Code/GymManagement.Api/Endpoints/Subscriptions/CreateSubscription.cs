using GymManagement.Application.Services;
using GymManagement.Contracts.Subscriptions;

namespace GymManagement.Api.Endpoints.Subscriptions
{
    public static class CreateSubscription
    {
        public static WebApplication AddCreatingSubscriptionEndpoint(this WebApplication app)
        {
            app.MapPost("/subscriptions", async (
                        CreateSubscriptionRequest request,
                        ISubscriptionsService subscriptionsService)
                    =>
                {
                    var subscriptionId =
                        subscriptionsService.CreateSubscription(request.SubscriptionType.ToString(), request.AdminId);

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