using Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API.Exceptions
{
    public class GlobalExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            ProblemDetails problem = exception switch
            {
                ValidationException ex => new ValidationProblemDetails(
                                    ex.Errors.GroupBy(g => g.PropertyName)
                                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
                                    )
                {
                    Title = "Validation failed",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = ex.Message,
                },
                DomainException dx => new ProblemDetails()
                {
                    Title = "Domain Error",
                    Detail = dx.Message,
                    Status = StatusCodes.Status400BadRequest
                },
                _ => new ProblemDetails()
                {
                    Title = "Server Error",
                    Detail = "An unhandled Exception",
                    Status = StatusCodes.Status500InternalServerError
                }
            };

            httpContext.Response.StatusCode = problem.Status!.Value;

            await problemDetailsService.WriteAsync(new ProblemDetailsContext()
            {
                HttpContext = httpContext,
                ProblemDetails = problem
            });

            return true;
        }
    }
}
