using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Behaviors;

public class UnhandledExceptionBehavior<TRequest, TResponse>: IPipelineBehavior<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TRequest> _logger;

    public UnhandledExceptionBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception e)
        {
            var requestName = nameof(TRequest);
            _logger.LogError(e, $"Unhandled exception occured with request name: {requestName}, {request}");
            throw;
        }
    }
}