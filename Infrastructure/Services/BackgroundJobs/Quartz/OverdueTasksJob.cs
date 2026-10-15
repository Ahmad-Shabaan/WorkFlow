using Application.Interfaces.Persistence;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Infrastructure.Services.BackgroundJobs.Quartz
{
    [DisallowConcurrentExecution]
    public sealed class OverdueTasksJob : IJob
    {
        private readonly ILogger _logger;
        private readonly ITaskRepository _taskRepository;
        public OverdueTasksJob(ILogger<OverdueTasksJob> logger, ITaskRepository taskRepository)
        {
            _logger = logger;
            _taskRepository = taskRepository;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var cancellationToken = context.CancellationToken;
            var jobKey = context.JobDetail.Key.ToString();
            var startedAt = DateTimeOffset.UtcNow;

            _logger.LogInformation(
                "Starting {JobName}. Fired at {FireTimeUtc}.",
                jobKey,
                context.FireTimeUtc);
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var tasks = await _taskRepository.GetOverdueTasks(cancellationToken);
                _logger.LogInformation("{JobName} found {OverdueTaskCount} overdue tasks.", jobKey, tasks.Count());
                
                foreach (var task in tasks)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    // if need do something with the task. but for now just log the task details
                    _logger.LogInformation("Processing overdue task with ID: {TaskId} and Name: {TaskName}, which is due on {EndDate}.", task.Id, task.TaskName, task.EndDate);
                }

                _logger.LogInformation("{JobName} completed successfully. " 
                    +"Found {OverdueTaskCount} overdue tasks in {ElapsedMilliseconds} ms.",
                    jobKey,tasks.Count(),(DateTimeOffset.UtcNow - startedAt).TotalMilliseconds);
            }
            catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested) // to add additional check for cancellation
            {
                _logger.LogWarning("Operation was canceled while executing {JobName}.", jobKey);
                throw; // Rethrow to let Quartz know the job was canceled.
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while executing {JobName}.", jobKey);
                throw; // Rethrow to let Quartz handle the exception as per its configuration.
            }

        }
    }
}
