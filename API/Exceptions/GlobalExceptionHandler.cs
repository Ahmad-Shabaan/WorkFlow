using Domain.Exceptions;
using FluentValidation;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

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
                    Title = "Break Rules",
                    Detail = dx.Message,
                    Status = StatusCodes.Status400BadRequest
                },
                InvalidCredentialException _ => new ProblemDetails()
                {
                    Title = "Invalid Credentials",
                    Detail = "Email or password is incorrect.",
                    Status = StatusCodes.Status401Unauthorized
                },
                NotFoundException dx => new ProblemDetails()
                {
                    Title = "Not Found",
                    Detail = dx.Message,
                    Status = StatusCodes.Status404NotFound
                },
                OperationFailedException dx => new ProblemDetails()
                {
                    Title = "Operation failed",
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
