using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling {RequestName}", typeof(TRequest).Name);
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            try
            {
                var response = await next(cancellationToken);
                sw.Stop();
                logger.LogInformation("Handled {RequestName} in {ElapsedMilliseconds}ms", typeof(TRequest).Name, sw.ElapsedMilliseconds);
                return response;
            }
            catch (Exception ex)
            {

                sw.Stop();
                logger.LogError(ex, "Error handling {RequestName} in {ElapsedMilliseconds}ms", typeof(TRequest).Name, sw.ElapsedMilliseconds);
                throw;
            }
        }
    }
}
