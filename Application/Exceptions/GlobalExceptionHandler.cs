using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Произошло исключение: {Message}", exception.Message);

        var (statusCode, title) = exception switch
        {
            ApplicationException => (StatusCodes.Status400BadRequest, "Ошибка приложения"),
            NotFoundException => (StatusCodes.Status404NotFound, "Ресурс не найден"),
            ValidationException => (StatusCodes.Status422UnprocessableEntity, "Ошибка валидации"),
            _ => (StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера")
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(new
        {
            Status = statusCode,
            Title = title,
            Detail = exception.Message
        }, cancellationToken);

        return true;
    }
}