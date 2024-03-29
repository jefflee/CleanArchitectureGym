using GymManagement.Contracts.Subscriptions;

namespace GymManagement.Api.Endpoints.Subscriptions
{
    public static class CreateSubscription
    {
        public static void AddCreatingSubscriptionEndpoint(this WebApplication app)
        {
            app.MapPost("/subscriptions", async (CreateSubscriptionRequest request) => { return Results.Ok(request); })
                .WithName("CreateASubscription")
                .WithOpenApi();
        }
    }
}