using Application.Features.Projects.DTOs;
using Application.Features.Projects.Queries;
using Dapper;
using Domain.Entities.ProjectAggregate;
using System.Data;
using Task = Domain.Entities.ProjectAggregate.Task;

namespace Infrastructure.Persistence.Queries
{
    public class ProjectQueries : IProjectQuries
    {
        private readonly IDbConnection _connection;
        public ProjectQueries(IDbConnection dbConnection)
        {
            _connection = dbConnection;
        }
        public async Task<Project?> GetProjectByPublicId(Guid publicId, CancellationToken cancellationToken)
        {
            string sql = """
                SELECT p.Id,p.PublicId,p.ProjectName,p.ProjectDescription,p.StartDate,p.EndDate,p.ProjectStatus,t.PublicId,t.TaskName,t.TaskDescription,t.StartDate,t.EndDate,t.TaskStatus,t.StartDate,t.EndDate
                FROM Projects p 
                LEFT JOIN Tasks t ON p.Id = t.ProjectId
                WHERE p.PublicId = @PublicId;
                """;
            var command = new CommandDefinition(sql, new { PublicId = publicId }, cancellationToken: cancellationToken);
            var lookup = new Dictionary<Guid, Project>();
            await _connection.QueryAsync<Project, Task, Project>(command, (project, task) =>
            {
                if (!lookup.TryGetValue(project.PublicId, out var existingProoject))
                {
                    existingProoject = project;
                    lookup.Add(project.PublicId, existingProoject);
                }
                if (task != null)
                    existingProoject.AddTask(task.TaskName, task.TaskDescription, task.StartDate, task.EndDate, task.PublicId, task.TaskStatus);

                return existingProoject;
            }, splitOn: "PublicId");

            return lookup.Values.FirstOrDefault();
        }

        public async Task<IReadOnlyList<Project>> GetAllProjects(CancellationToken cancellationToken)
        {
            string sql = """
                SELECT * 
                FROM Projects
                """;
            var command = new CommandDefinition(sql, cancellationToken: cancellationToken);
            var result = await _connection.QueryAsync<Project>(command);
            return result.ToList();
        }
    }
}
