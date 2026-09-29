using Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
namespace Infrastructure.Persistence.Seed
{
    public class Seed(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager, ILogger<Seed> logger)
    {

        public async Task Seeder()
        {
            await SeedRolesAsync();
            await SeedUsersAsync();
            //... any table
        }

        private async Task SeedRolesAsync()
        {
            string[] roles =
       {
            "Admin",
            "Manager",
            "Employee"
        };
            
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<int>
                    {
                        Name = role,
                    });
                }
            }
        }
        private async Task SeedUsersAsync()
        {
            try
            {
                if (!userManager.Users.Any())
                {
                    var path = Path.Combine(AppContext.BaseDirectory, "Persistence", "SeedData", "users.json");
                    var users = await LoadDataAsync<ApplicationUser>(path, logger);
                    if (users != null && users.Any())
                    {
                        foreach (var user in users)
                        {
                            var result = await userManager.CreateAsync(user, "Pa$$w0rd");
                            if (result.Succeeded)
                                logger.LogInformation($"User {user.UserName} created successfully.");
                            else
                                logger.LogError($"Failed to create user {user.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        }
                    }
                    else
                        logger.LogWarning("No users found to seed.");

                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the AppIdentity database.");
                throw;
            }
        }

        private static async Task<List<T>?> LoadDataAsync<T>(string filePath, ILogger<Seed> logger)
        {
            try
            {
                // data in json , xml , excel ,..
                var data = await File.ReadAllTextAsync(filePath);
                // convert each obj to element"
                var elements = JsonSerializer.Deserialize<List<T>>(data);
                if (elements == null)
                    logger.LogWarning("Deserialization returned null. Check the JSON file format.");
                return elements;
            }
            catch (FileNotFoundException ex)
            {
                logger.LogError(ex, $"The file {filePath} was not found.");
                throw;
            }
            catch (JsonException ex)
            {
                logger.LogError(ex, $"Error deserializing the JSON file {filePath}.");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An unexpected error occurred while reading the file {filePath}.");
                throw;
            }
        }
    }
}
