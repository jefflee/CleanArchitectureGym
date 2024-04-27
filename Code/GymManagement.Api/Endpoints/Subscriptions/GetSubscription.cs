using GymManagement.Application.Subscriptions.Queries.GetSubscription;
using GymManagement.Contracts.Subscriptions;
using MediatR;

namespace GymManagement.Api.Endpoints.Subscriptions
{
    public static class GetSubscription
    {
        public static WebApplication AddGettingSubscriptionEndpoint(this WebApplication app)
        {
            app.MapGet("/subscriptions/{subscriptionId}", async (
                        Guid subscriptionId,
                        ISender mediator)
                    =>
                {
                    var query = new GetSubscriptionQuery(subscriptionId);

                    var getSubscriptionsResult = await mediator.Send(query);

                    return getSubscriptionsResult.MatchFirst(
                        subscription =>
                        {
                            var response = new SubscriptionResponse(
                                subscription.Id,
                                SubscriptionUtility.ToDto(subscription.SubscriptionType));
                            return Results.Ok(response);
                        },
                        error => Results.Problem(error.Description));
                })
                .WithName("GetSubscription")
                .WithTags("Subscription")
                .WithOpenApi();

            return app;
        }
    }
}