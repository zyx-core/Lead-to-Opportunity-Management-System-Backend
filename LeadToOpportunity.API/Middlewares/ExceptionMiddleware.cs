using System.Text.Json;
using LeadToOpportunity.Shared.Exception;
using LeadToOpportunity.Shared.Exceptions;
using LeadToOpportunity.Shared.Responses;

namespace LeadToOpportunity.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse();

        switch (exception)
        {
            case BadRequestException:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response.StatusCode = 400;
                break;

            case UnauthorizedException:
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                response.StatusCode = 401;
                break;

            case NotFoundException:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                response.StatusCode = 404;
                break;

            case ConflictException:
                context.Response.StatusCode = StatusCodes.Status409Conflict;
                response.StatusCode = 409;
                break;

            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response.StatusCode = 500;
                break;
        }

        response.Message = exception.Message;

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }
}