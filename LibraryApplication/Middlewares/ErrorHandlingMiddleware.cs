using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

public class ErrorHandlingMiddleware
{
    // Middleware'in pipeline'da sonraki adıma geçmesini sağlar
    private readonly RequestDelegate _next;
    // Loglama için ILogger servisi
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    // Middleware constructor'ı. Gerekli servisler enjekte edilir.
    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    // InvokeAsync, middleware'in ana işlevini yerine getirir.
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            // Sonraki middleware'e geçiş yapılır.
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            // Hata durumunda hatayı loglar ve kullanıcıya hata yanıtını hazırlar.
            _logger.LogError($"Something went wrong: {ex}");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    // Hata durumunda HTTP yanıtını hazırlayan yardımcı metot.
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        return context.Response.WriteAsync(new ErrorDetails()
        {
            StatusCode = context.Response.StatusCode,
            Message = "Internal Server Error from the custom middleware."
        }.ToString());
    }
}

// HTTP yanıtı için hata detaylarını içeren yardımcı class.
public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }

    public override string ToString()
    {
        return $"{{\"StatusCode\": {StatusCode}, \"Message\": \"{Message}\"}}";
    }
}
