using ErrorOr;
using MediatR;

namespace GymManagement.Application.Gyms.AddTrainer;

public record AddTrainerCommand(Guid SubscriptionId, Guid GymId, Guid TrainerId)
    : IRequest<ErrorOr<Success>>;