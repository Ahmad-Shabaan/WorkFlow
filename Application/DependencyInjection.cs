using Application.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // for register MediatR in DI Container
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

                // Register ValidationBehavior instead of using old way (AddTransient)
                options.AddOpenBehavior(typeof(ValidationBehavior<,>)); 
                options.AddOpenBehavior(typeof(LoggingBehavior<,>));
            }); 


            // Register ValidationBehavior DI Will create instance for each time mediatr send req (old way)
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly); // for register Fluent Validation in DI Container

            return services;
        }
    }
}
