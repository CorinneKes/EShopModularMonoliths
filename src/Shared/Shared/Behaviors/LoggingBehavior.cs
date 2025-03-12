using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Shared.Behaviors;

public class LoggingBehavior<TRequest, TResponse> 
    (ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("[START] Handle request={Request} -  Response={Response} - RequestData={RequestData}",
            typeof(TRequest).Name, typeof(TResponse).Name, request); 
        
        var timer = new Stopwatch();
        timer.Start();

        var response = await next(); // Measures the duration of each request (including MediatR pipeline)

        timer.Stop();

        var timeTaken = timer.Elapsed;
        if (timeTaken.Seconds > 3) // log a warning if the request takes longer than 3 seconds
        {
            logger.LogWarning("[PERFORMANCE] The request {Request} took {timeTaken} seconds.",
            typeof(TRequest).Name, timeTaken.Seconds); 
        }

        logger.LogInformation("[END] Handled {Request} with {Response}", typeof(TRequest).Name, typeof(TResponse).Name);
        return response;
    }   
}
