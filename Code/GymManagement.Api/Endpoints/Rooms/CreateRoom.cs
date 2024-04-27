using GymManagement.Application.Rooms.Commands.CreateRoom;
using GymManagement.Contracts.Room;
using MediatR;

namespace GymManagement.Api.Endpoints.Rooms;

public static class CreateRoom
{
    public static WebApplication AddCreatingRoomEndpoint(this WebApplication app)
    {
        app.MapPost("/gyms/{gymId}/rooms", async (
                    CreateRoomRequest request,
                    Guid gymId,
                    ISender mediator)
                =>
            {
                var command = new CreateRoomCommand(
                    gymId,
                    request.Name);

                var createRoomResult = await mediator.Send(command);

                return createRoomResult.MatchFirst(
                    room => Results.Created(
                        $"rooms/{room.Id}", // todo: add host
                        new RoomResponse(room.Id, room.Name)),
                    error => Results.Problem(error.Description));
            })
            .WithName("CreateRoom")
            .WithTags("Room")
            .WithOpenApi();

        return app;
    }
}