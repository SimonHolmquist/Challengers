using FluentValidation;

namespace Challengers.Api.Middleware;
public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = ContentTypeJson;

            var response = new
            {
                message = GetMessage(ValidationFailedMessage),
                errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
            };

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (ArgumentException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = ContentTypeJson;
            await context.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Response.ContentType = ContentTypeJson;
            await context.Response.WriteAsJsonAsync(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = ContentTypeJson;
            await context.Response.WriteAsJsonAsync(new { message = GetMessage(InternalServerErrorMessage) });
        }
    }
}
