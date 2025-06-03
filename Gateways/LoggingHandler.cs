using Microsoft.Extensions.Logging;

namespace Gateways;

public class LoggingHandler : DelegatingHandler
{
    private readonly ILogger<LoggingHandler> _logger;

    public LoggingHandler(ILogger<LoggingHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("HTTP Request: {Method} {Url}", request.Method, request.RequestUri);

        var response = await base.SendAsync(request, cancellationToken);

        _logger.LogInformation("HTTP Response: {StatusCode} for {Url}", response.StatusCode, request.RequestUri);

        return response;
    }
}
