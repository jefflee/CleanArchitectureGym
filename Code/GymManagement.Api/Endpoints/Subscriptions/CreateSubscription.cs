using GymManagement.Application.Subscriptions.Commands.CreateSubscription;
using GymManagement.Contracts.Subscriptions;
using MediatR;
using DomainSubscriptionType = GymManagement.Domain.Subscriptions.SubscriptionType;

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
                    if (!DomainSubscriptionType.TryFromName(
                            request.SubscriptionType.ToString(),
                            out var subscriptionType))
                    {
                        return Results.Problem(
                            statusCode: StatusCodes.Status400BadRequest,
                            detail: "Invalid subscription type");
                    }

                    var command = new CreateSubscriptionCommand(
                        subscriptionType,
                        request.AdminId);

                    var createSubscriptionResult = await mediator.Send(command);

                    return createSubscriptionResult.MatchFirst(
                        subscription =>
                        {
                            var response = new SubscriptionResponse(
                                subscription.Id, SubscriptionUtility.ToDto(subscription.SubscriptionType));
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