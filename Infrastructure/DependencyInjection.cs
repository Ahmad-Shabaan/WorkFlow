using Application.Features.Projects.Queries;
using Application.Features.Tasks.Queries;
using Application.Interfaces.Persistence;
using Infrastructure.Persistence.Queries;
using Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {

            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IProjectQuries, ProjectQueries>();
            services.AddScoped<ITaskQuries, TaskQueries>();
            return services;
        }
    }
}
