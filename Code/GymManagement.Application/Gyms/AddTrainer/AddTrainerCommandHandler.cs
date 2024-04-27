using ErrorOr;
using GymManagement.Application.Common.Interfaces;
using MediatR;

namespace GymManagement.Application.Gyms.AddTrainer;

public class AddTrainerCommandHandler(IGymsRepository gymsRepository,
        IUnitOfWork unitOfWork)
    : IRequestHandler<AddTrainerCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(AddTrainerCommand command, CancellationToken cancellationToken)
    {
        var gym = await gymsRepository.GetByIdAsync(command.GymId);

        if (gym is null)
        {
            return Error.NotFound(description: "Gym not found");
        }

        var addTrainerResult = gym.AddTrainer(command.TrainerId);

        if (addTrainerResult.IsError)
        {
            return addTrainerResult.Errors;
        }

        await gymsRepository.UpdateGymAsync(gym);
        await unitOfWork.CommitChangesAsync();

        return Result.Success;
    }
}