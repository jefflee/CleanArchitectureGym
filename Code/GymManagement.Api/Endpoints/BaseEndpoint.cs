using ErrorOr;

namespace GymManagement.Api.Endpoints;

public class BaseEndpoint
{
    internal static IResult CustomProblem(List<Error> errors)
    {
        if (errors.Count is 0)
        {
            return Results.Problem();
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        return CustomProblem(errors[0]);
    }

    internal static IResult CustomProblem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        return Results.Problem(statusCode: statusCode, detail: error.Description);
    }

    internal static IResult ValidationProblem(List<Error> errors)
    {
        // if using ApiController
        //var modelStateDictionary = new ModelStateDictionary();

        //foreach (var error in errors)
        //{
        //    modelStateDictionary.AddModelError(
        //        error.Code,
        //        error.Description);
        //}

        //return ValidationProblem( modelStateDictionary);

        var modelStateDictionary =
            errors.ToDictionary(
                error => error.Code,
                error => new[] { error.Description }
            );

        return Results.ValidationProblem(modelStateDictionary);
    }
}