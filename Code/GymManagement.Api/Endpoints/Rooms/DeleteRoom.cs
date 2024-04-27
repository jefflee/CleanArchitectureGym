using GymManagement.Application.Rooms.Commands.DeleteRoom;
using MediatR;

namespace GymManagement.Api.Endpoints.Rooms;

public static class DeleteRoom
{
    public static WebApplication AddDeletingRoomEndpoint(this WebApplication app)
    {
        app.MapDelete("/gyms/{gymId}/rooms/{roomId}", async (
                    Guid gymId,
                    Guid roomId,
                    ISender mediator)
                =>
            {
                var command = new DeleteRoomCommand(gymId, roomId);

                var deleteRoomResult = await mediator.Send(command);

                return deleteRoomResult.MatchFirst<IResult>(
                    _ => Results.NoContent(),
                    error => Results.Problem(error.Description));
            })
            .WithName("DeleteRoom")
            .WithTags("Room")
            .WithOpenApi();

        return app;
    }
}