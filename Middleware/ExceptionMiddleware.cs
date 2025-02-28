using System.Net;
using System.Text.Json;

public class ExceptionMiddleware {
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger) {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext) {
        try {
            await _next(httpContext);
        } catch (Exception ex) {
            _logger.LogError($"Algo salió mal: {ex}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception) {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new {
            StatusCode = context.Response.StatusCode,
            Message = "Error interno del servidor del middleware personalizado.",
            Detailed = exception.Message
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
