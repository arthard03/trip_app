namespace trip_app.OutputFolder.Middlewares;

using Microsoft.AspNetCore.Mvc;


public class ExceptionHandingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandingMiddleware> _logger;

    public ExceptionHandingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandingMiddleware> logger)
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
            _logger.LogError(ex, ex.Message);
            
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server error"
            };

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}