using System.Net;
using System.Text.Json;

namespace movie_tracker.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Сталася помилка: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        // Універсальний мапінг C# винятків у HTTP статус-коди
        var statusCode = exception switch
        {
            ArgumentException => (int)HttpStatusCode.BadRequest,            // 400 (Невірні дані)
            KeyNotFoundException => (int)HttpStatusCode.NotFound,           // 404 (Не знайдено)
            InvalidOperationException => (int)HttpStatusCode.Conflict,      // 409 (Конфлікт, наприклад, дублікат)
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,// 401 (Немає доступу)
            _ => (int)HttpStatusCode.InternalServerError                    // 500 (Все інше)
        };

        context.Response.StatusCode = statusCode;

        // Формуємо відповідь для клієнта
        var response = new
        {
            StatusCode = statusCode,
            Error = exception.Message
        };

        var jsonResponse = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(jsonResponse);
    }
}