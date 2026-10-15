using API;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
namespace IntegrationTest
{
    public sealed class WorkflowApiFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {

                builder.ConfigureTestServices(service =>
                {
                    service.RemoveAll(typeof(DbContextOptions<AppDbContext>));
                    service.AddDbContext<AppDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestWorkflow");
                    });
                });
        }
    }
}
