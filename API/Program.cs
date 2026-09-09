using API.Exceptions;
using API.options;
using Application;
using Application.Interfaces.Persistence;
using Application.Specifications;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using BookHavenAPI.Errors;
using Infrastructure;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddProblemDetails();

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

            // Register services to the DI container which the engine created before.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            // explore the API endpoints and their metadata for generating Swagger documentation.
            builder.Services.AddEndpointsApiExplorer();
            // This line registers the Swagger generator, which creates Swagger documents for your API.
            //At this point it's a blank/default generator — it doesn't know your versions yet.
            builder.Services.AddSwaggerGen();
            //It tells the DI container: "When SwaggerGenOptions is needed, also run ConfigureSwaggerOptions.Configure() against it."
            // This line adds a custom configuration for Swagger options, which will be used to set up the Swagger documentation according to your API versioning scheme.
            //creates a separate SwaggerDoc for each one (e.g., v1, v2).
            builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();


            // API versioning configuration
            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;// it works with QueryString or Header readers:
                options.ReportApiVersions = true; // Adds version information to every response header automatically.
                options.ApiVersionReader = new UrlSegmentApiVersionReader(); // Tells the versioning system where to look for the version number in incoming requests

            }).AddApiExplorer(options =>
            //The API Explorer is what makes versioning metadata visible to Swagger/OpenAPI
            //without this, IApiVersionDescriptionProvider wouldn't exist and ConfigureSwaggerOptions would break.
            {
                options.GroupNameFormat = "'v'VVV"; //  format of the group name that Swagger uses to identify each version. v2.0
                options.SubstituteApiVersionInUrl = true; //  it replaces the {version} placeholder in the route template with the actual version number.
            });

            // allow dependency injection for db context        
            builder.Services.AddDbContext<AppDbContext>
                (options => options.UseSqlServer(builder.Configuration.GetConnectionString("WorkFlow") ?? throw new InvalidOperationException("Connection string 'WorkFlow' not found.")));

            builder.Services.AddScoped<IDbConnection>((sp) => 
                                                    new SqlConnection(builder.Configuration.GetConnectionString("WorkFlow") ??
                                                    throw new InvalidOperationException("Connection string 'WorkFlow' not found.")));
            
            // register Application services
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices();
            // allow dependency injection for unit of work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<ISpecificationEvaluator, SpecificationEvaluator>();

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            builder.Services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());

            // Get API Model state validation errors
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState.Where(m => m.Value?.Errors.Count > 0) // null check
                                                         .SelectMany(m => m.Value!.Errors)      // null-forgiving operator
                                                         .Select(e => e.ErrorMessage).ToArray();
                    return new BadRequestObjectResult(new APIValidationErrorResponse() { Errors = errors });
                };
            });


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {

                    policy.WithOrigins("https://book-wise-ecru.vercel.app", "https://localhost:5173")  // React origin ClientUrl
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowCredentials();
                    //policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();//WithOrigins("https://localhost:4200");
                });
            });



            var app = builder.Build();

            // app is internal web server (kestrel)
            app.UseExceptionHandler();



            // i need create obj like how clr do 
            //create scope is unmanaged code(resource) use try finally or using
            using var scope = app.Services.CreateScope();
            var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
            try
            {
                //create obj from context (this code clr will run it to create context)
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await context.Database.MigrateAsync(); // to apply mirgations
                                                       // seeding data
                                                       //await BookHavenDbContextSeeding.SeedAsync(context, loggerFactory.CreateLogger<BookHavenDbContextSeeding>());


            }
            catch (Exception ex)
            {
                loggerFactory.CreateLogger<Program>().LogError(ex, "An error occurred during migration");
            }


            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                // This line retrieves the API version descriptions from the service provider, which will be used to configure the Swagger UI.
                //It holds a list of ApiVersionDescription objects, one per version found in your app.
                var porvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                // These lines enable the Swagger middleware, which generates the Swagger JSON documents and serves the Swagger UI.
                app.UseSwagger();
                // This block configures the Swagger UI to display an endpoint for each API version, using the group names defined in the ApiExplorer settings.
                app.UseSwaggerUI(options =>
                {
                    //Loops over every discovered API version (v1, v2, etc.) to register a UI tab for each one.
                    foreach (var description in porvider.ApiVersionDescriptions)
                    {
                        //description.GroupName → e.g., "v2" (set via [ApiExplorerSettings(GroupName = "v2")] in your HelloController).
                        //The endpoint URL → / swagger / v2 / swagger.json.
                        //The label in the UI dropdown → "V2"(uppercased).
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });
            }
            app.UseForwardedHeaders();


            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx => ctx.Context.Response.Headers.CacheControl =
                        "public,max-age=31536000,immutable"
            });
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
