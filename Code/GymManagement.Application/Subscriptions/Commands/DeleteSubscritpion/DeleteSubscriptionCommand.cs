using ErrorOr;
using MediatR;

namespace GymManagement.Application.Subscriptions.Commands.DeleteSubscritpion;

public record DeleteSubscriptionCommand(Guid SubscriptionId) : IRequest<ErrorOr<Deleted>>;