using System.Diagnostics;
using System.Text;

namespace GameService.Infrastructure.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Capture the start time
        var startTime = DateTime.UtcNow;
        var requestId = Activity.Current?.Id ?? context.TraceIdentifier;

        try
        {
            // Log the request details
            _logger.LogInformation(
                "Request {RequestMethod} {RequestPath} started - RequestId: {RequestId}, IP: {IpAddress}, User: {User}, QueryString: {QueryString}",
                context.Request.Method,
                context.Request.Path,
                requestId,
                context.Connection.RemoteIpAddress,
                context.User.Identity?.Name ?? "Anonymous",
                context.Request.QueryString);

            // Optionally log request headers
            // LogHeaders(context.Request.Headers, "Request");

            // Optionally enable request body logging (consider security implications)
            // await LogRequestBody(context.Request);
            
            // Call the next middleware in the pipeline
            await _next(context);
            
            // Calculate duration
            var duration = DateTime.UtcNow - startTime;
            
            // Log the response details with success
            _logger.LogInformation(
                "Response {StatusCode} for {RequestMethod} {RequestPath} completed in {Duration}ms - RequestId: {RequestId}",
                context.Response.StatusCode,
                context.Request.Method,
                context.Request.Path,
                duration.TotalMilliseconds,
                requestId);
        }
        catch (Exception ex)
        {
            // Calculate duration
            var duration = DateTime.UtcNow - startTime;
            
            // Log the exception
            _logger.LogError(
                ex,
                "Request {RequestMethod} {RequestPath} failed after {Duration}ms with error: {ErrorMessage} - RequestId: {RequestId}",
                context.Request.Method,
                context.Request.Path,
                duration.TotalMilliseconds,
                ex.Message,
                requestId);
                
            throw; // Re-throw to let exception middleware handle it
        }
    }

    // Optional methods for more detailed logging
    private void LogHeaders(IHeaderDictionary headers, string type)
    {
        foreach (var header in headers)
        {
            // Skip logging sensitive headers like Authorization
            if (!header.Key.Equals("Authorization", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogDebug("{Type} Header: {Key}: {Value}", type, header.Key, header.Value);
            }
        }
    }

    private async Task LogRequestBody(HttpRequest request)
    {
        // Enable buffering so the body can be read multiple times
        request.EnableBuffering();
        
        using var reader = new StreamReader(
            request.Body,
            encoding: Encoding.UTF8,
            detectEncodingFromByteOrderMarks: false,
            leaveOpen: true);
            
        var body = await reader.ReadToEndAsync();
        
        // Log the body if it's not too large
        if (!string.IsNullOrEmpty(body) && body.Length < 1000)
        {
            _logger.LogDebug("Request Body: {Body}", body);
        }
        
        // Reset the position to the beginning
        request.Body.Position = 0;
    }
}