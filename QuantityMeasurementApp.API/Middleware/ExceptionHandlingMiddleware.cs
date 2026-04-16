using System.Net;
using System.Text.Json;
using QuantityMeasurementAppBusiness.Exceptions;
using QuantityMeasurementAppRepository.Exceptions;

namespace QuantityMeasurementApp.API.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (QuantityMeasurementException ex)
        {
            _logger.LogWarning(ex, "Domain error");
            await WriteProblemAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (NotSupportedException ex)
        {
            _logger.LogWarning(ex, "Operation not supported");
            await WriteProblemAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (DatabaseException ex)
        {
            _logger.LogError(ex, "Database error");
            await WriteProblemAsync(context, HttpStatusCode.InternalServerError,
                "A database error occurred.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error");
            await WriteProblemAsync(context, HttpStatusCode.InternalServerError,
                "An unexpected error occurred.");
        }
    }

    private static async Task WriteProblemAsync(
        HttpContext context,
        HttpStatusCode status,
        string message)
    {
        if (context.Response.HasStarted)
            return;

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;

        var body = new
        {
            status = (int)status,
            error = status.ToString(),
            message
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(body));
    }
}
