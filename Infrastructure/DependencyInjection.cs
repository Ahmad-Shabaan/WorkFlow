using Application.Interfaces.Common;
using Application.Interfaces.Persistence;
using Infrastructure.Options;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Entities;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence.Seed;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // allow dependency injection for db context        
            services.AddDbContext<AppDbContext>
                (options => options.UseSqlServer(configuration.GetConnectionString("WorkFlow") ?? throw new InvalidOperationException("Connection string 'WorkFlow' not found.")));

            var jwtOptions = configuration.GetSection(JwtOption.SectionName).Get<JwtOption>()!;



            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;

                options.User.RequireUniqueEmail = true;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

                options.SignIn.RequireConfirmedEmail = false;
            })
                .AddRoles<IdentityRole<int>>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders()
                 .AddSignInManager();


            //services.AddAuthentication
            services.AddAuthentication(options =>
            {
                // JWT Bearer scheme will create and validate token
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                // configure JWT bearer options here if needed
                options.SaveToken = true;

                // validate token
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.ValidAudience,
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.ValidIssuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                //  اقرا التوكن من الكوكي مش من الهيدر
                //options.Events = new JwtBearerEvents
                //{
                //    OnMessageReceived = context =>
                //    {
                //        context.Token = context.Request.Cookies["access_token"];
                //        return Task.CompletedTask;
                //    }
                //};

            });



            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITaskAssignmentRepository, TaskAssignmentRepository>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<JwtService>();
            services.AddScoped<RefreshTokenService>();
            services.AddOptions<JwtOption>().Bind(configuration.GetSection(JwtOption.SectionName))
                    .ValidateDataAnnotations().ValidateOnStart();

            services.AddScoped<Seed>();

            return services;
        }
    }
}
