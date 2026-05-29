using System.Diagnostics;

namespace RachaStats.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;
    
    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        
        _logger.LogInformation("START {Method} {Path}", 
            context.Request.Method, 
            context.Request.Path);
        
        await _next(context);
        
        sw.Stop();
        
        _logger.LogInformation("END {Method} {Path} - {StatusCode} in {Elapsed} ms", 
            context.Request.Method, 
            context.Request.Path, 
            context.Response.StatusCode, 
            sw.ElapsedMilliseconds);
    }
}