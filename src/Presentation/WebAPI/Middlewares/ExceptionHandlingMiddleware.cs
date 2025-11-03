namespace WebAPI.Middlewares;

using System.Text.Json;
using Domain.Exceptions;

public class ExceptionHandlingMiddleware : IMiddleware {
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) => _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
        try{ await next(context); }
        catch (Exception e){
            _logger.LogError(e, e.Message);

            await HandleExceptionAsync(context, e);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception) {
        httpContext.Response.ContentType = "application/json";

        httpContext.Response.StatusCode = exception switch
        {
            BadRequestException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        var errors = Array.Empty<ApiError>();

        while (exception.InnerException != null){
            errors.Append(new ApiError(exception.Source, exception.Message));
            exception = exception.InnerException;
        }

        var response = new
        {
            status = httpContext.Response.StatusCode,
            message = exception.Message,
            errors
        };

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private record ApiError(string PropertyName, string ErrorMessage);
}