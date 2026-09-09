using Application.Features.Tasks.Queries;
using Dapper;
using System.Data;
namespace Infrastructure.Persistence.Queries
{
    public class TaskQueries : ITaskQuries
    {
        private readonly IDbConnection _connection;
        public TaskQueries(IDbConnection dbConnection)
        {
            _connection = dbConnection;
        }
        public async Task<IEnumerable<Domain.Entities.ProjectAggregate.Task>> GetAllAsync(CancellationToken cancellationToken)
        {
            string sql = """
                SELECT * 
                FROM Tasks
                """;
            var command = new CommandDefinition(sql, cancellationToken: cancellationToken);
            var result = await _connection.QueryAsync<Domain.Entities.ProjectAggregate.Task>(command);
            return result.ToList();
        }

        public Task<IEnumerable<Domain.Entities.ProjectAggregate.Task>> GetAllTasksInProjectAsync(Guid projectPublicId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Domain.Entities.ProjectAggregate.Task?> GetTaskByIdAsync(Guid taskId, CancellationToken cancellationToken)
        {
            string sql = """
                SELECT * 
                FROM Tasks
                WHERE PublicId = @TaskId
                """;
            var command = new CommandDefinition(sql, new { TaskId = taskId }, cancellationToken: cancellationToken);
            var result = await _connection.QueryAsync<Domain.Entities.ProjectAggregate.Task>(command);
            return result.FirstOrDefault();
        }
    }
}
