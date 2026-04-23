
using System.Text.Json;
using Dalleni.Domin.ResponsePattern;
using Dalleni.Domin.Exceptions;
using Dalleni.Domin.Interfaces.Handlers;

namespace Dalleni.API.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;
    private readonly IResponseHandler _responseHandler;
    public ErrorHandlerMiddleware(RequestDelegate next,
                                  ILogger<ErrorHandlerMiddleware> logger,
                                  IResponseHandler responseHandler)
    {
        _next = next;
        _logger = logger;
        _responseHandler = responseHandler;
    }

    public async Task Invoke(HttpContext context)
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

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        Response<object> response;

        // Map exception type to response
        switch (exception)
        {
            case DomainException domainEx:
                response = _responseHandler.BadRequest<object>(domainEx.Message);
                break;

            case DatabaseException dbEx:
                response = _responseHandler.ServerError<object>(dbEx.Message);
                break;

            case NotFoundException notFoundEx:
                response = _responseHandler.NotFound<object>(notFoundEx.Message);
                break;

            case BadRequestException badReqEx:
                response = _responseHandler.BadRequest<object>(badReqEx.Message);
                break;

            case ValidationException validationEx:
                response = _responseHandler.UnprocessableEntity<object>(validationEx.Message);
                break;

            case UnauthorizedAccessException:
                response = _responseHandler.Unauthorized<object>();
                break;

            default:
                response = _responseHandler.ServerError<object>(exception.Message);
                break;
        }

        // Structured logging
        _logger.LogError(exception,
            "Exception occurred. StatusCode: {StatusCode}, Message: {Message}",
            (int)response.StatusCode, response.Message);

        // Return JSON response
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)response.StatusCode;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
    }
}