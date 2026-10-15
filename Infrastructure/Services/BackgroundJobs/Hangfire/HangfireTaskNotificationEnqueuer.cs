using Application.Interfaces.BackgroundJobs.Hangfire;
using Hangfire;
using Microsoft.Extensions.Logging;
namespace Infrastructure.Services.BackgroundJobs.Hangfire
{
    public class HangfireTaskNotificationEnqueuer : ITaskNotificationScheduler
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        public HangfireTaskNotificationEnqueuer(IBackgroundJobClient backgroundJobClient)
        {
            _backgroundJobClient = backgroundJobClient;
        }
        public void EnqueueTaskCompletedNotification(Guid taskId, CancellationToken cancellationToken)
        {
            _backgroundJobClient.Enqueue<TaskNotificationJob>(job => job.ExecuteAsync(taskId, cancellationToken));
        }
    }
}
