using ErrorOr;
using MediatR;

namespace GymManagement.Application.Gyms.DeleteGym;

public record DeleteGymCommand(Guid SubscriptionId, Guid GymId) : IRequest<ErrorOr<Deleted>>;