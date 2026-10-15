using Application.Features.Authentication.DTOs;
using Domain.Enums;
using Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace IntegrationTest
{
    public class TestWorkFlow : IClassFixture<WorkflowApiFactory>
    {
        private const string Password = "P@ssw0rd";

        private readonly WorkflowApiFactory _factory;
        private readonly HttpClient _client;


        public TestWorkFlow(WorkflowApiFactory factory)
        {
            _factory = factory;
            _client =_factory.CreateClient();
        }

        [Fact]
        public async System.Threading.Tasks.Task CompleteTaskWorkflow_RegisterToCompleteTask_Succeeds()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            // seeding roles
            await EnsureRoleExistsAsync(roleManager, UserRoles.Admin.ToString());
            await EnsureRoleExistsAsync(roleManager, UserRoles.Manager.ToString());
            await EnsureRoleExistsAsync(roleManager, UserRoles.Employee.ToString());

            // seeding users (admin,manager)
            var admin = await CreateUserAsync(userManager, "admin@gmail.com","admin",UserRoles.Admin.ToString());
            var manager = await CreateUserAsync(userManager, "manager@gmail.com", "manager", UserRoles.Manager.ToString());
            
            var now = DateTimeOffset.Now;

            // 1. Register
            var registerData = new
            {
                FirstName = "Ahmed",
                LastName = "Shabaan",
                Email = "ahmed@gmail.com",
                Password
            };
            using var registerResponse = await _client.PostAsJsonAsync("/api/v1/account/register", registerData);
            Assert.Equal(HttpStatusCode.OK, registerResponse.StatusCode);

            var employee = await userManager.FindByEmailAsync("ahmed@gmail.com");
            Assert.NotNull(employee);
            var employeeId = employee.PublicId;

            // 2. Login
            var adminAccessToken = await LoginAsync("admin@gmail.com");
            SetBearerToken(adminAccessToken);



            // 3. Create Project
            var projectData = new
            {
                ProjectName = "Project 1",
                ProjectDescription = "Project Description",
                StartDate = now.AddDays(1),
                EndDate = now.AddDays(10),
                ManagerId = manager?.PublicId

            };

            // act
            using var createProjectResponse = await _client.PostAsJsonAsync("/api/v1/projects", projectData);
            
            // assert
            var projectId = GetCreatedResourceId(createProjectResponse);




            // 4. Create Task
            // Use ProjectId and extract TaskId.
            var taskData = new
            {
                ProjectId = projectId,
                TaskName = "Task",
                TaskDescription = "Task Description",
                StartDate = now.AddDays(2),
                EndDate = now.AddDays(5)
            };

            // act
            using var createTaskResponse = await _client.PostAsJsonAsync("/api/v1/tasks", taskData);
            // assert
            var taskId = GetCreatedResourceId(createTaskResponse);



            // 5. Assign Task

            // login as manager first
            var managerAccessToken = await LoginAsync("manager@gmail.com");
            SetBearerToken(managerAccessToken);


            // assign task now
            var assingEmployeeData = new
            {
                TaskId = taskId,
                UserId = employeeId
            };

            //act
            using var assignEmployeeResponse = await _client.PatchAsJsonAsync("/api/v1/tasks/assign-employee", assingEmployeeData);

            // assert
            Assert.Equal(HttpStatusCode.NoContent, assignEmployeeResponse.StatusCode);




            // 6. Complete Task
            var completeTaskData = new
            {
                TaskId = taskId,
                ProjectId = projectId,
            };
            //act
            using var completeTaskResponse = await _client.PatchAsJsonAsync("/api/v1/tasks/complete-task", completeTaskData);
            // assert
            Assert.Equal(HttpStatusCode.NoContent, completeTaskResponse.StatusCode);

        }


        private async Task<string> LoginAsync(string email)
        {
            _client.DefaultRequestHeaders.Authorization = null;

            var loginData = new
            {
                Email = email,
                Password
            };

            using var response = await _client.PostAsJsonAsync("/api/v1/account/login",loginData);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.Content.ReadFromJsonAsync<LoginDto>();

            Assert.NotNull(result);
            Assert.False(string.IsNullOrWhiteSpace(result.AccessToken));

            return result.AccessToken;
        }

        private void SetBearerToken(string accessToken)
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
        }




        private static Guid GetCreatedResourceId(HttpResponseMessage response)
        {
            Assert.Equal(HttpStatusCode.Created,response.StatusCode);
            var location = response.Headers.Location;
            Assert.NotNull(location);
            var lastSegment = location!.ToString().TrimEnd('/').Split('/').Last();
            return Guid.Parse(lastSegment);
        }

        private static async System.Threading.Tasks.Task EnsureRoleExistsAsync(RoleManager<IdentityRole<int>> roleManager,string roleName)
        {
            if (await roleManager.RoleExistsAsync(roleName))
                return;
            var result = await roleManager.CreateAsync(new IdentityRole<int> { Name = roleName });
            Assert.True(result.Succeeded,string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        private static async Task<ApplicationUser> CreateUserAsync(UserManager<ApplicationUser> userManager,string email,string userName,string role)
        {
            var user = new ApplicationUser
            {
                FirstName = userName,
                LastName = "User",
                Email = email,
                UserName = userName
            };

            var createResult = await userManager.CreateAsync(user,Password);
            Assert.True(createResult.Succeeded,string.Join(", ", createResult.Errors.Select(e => e.Description)));
            var roleResult = await userManager.AddToRoleAsync(user, role);
            Assert.True(roleResult.Succeeded,string.Join(", ", roleResult.Errors.Select(e => e.Description)));

            return user;
        }
    }

}