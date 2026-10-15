
using Application.Interfaces.BackgroundJobs.Hangfire;
using Application.Interfaces.Persistence;
using Hangfire;
using Infrastructure.Exceptions;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.BackgroundJobs.Hangfire
{
    public sealed class TaskNotificationJob : ITaskNotificationJob
    {
        private readonly ILogger _logger;
        private readonly ITaskRepository _taskRepository;

        public TaskNotificationJob(ILogger<TaskNotificationJob> logger, ITaskRepository taskRepository)
        {
            _logger = logger;
            _taskRepository = taskRepository;
        }
        [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Fail, ExceptOn = new[] { typeof(NotFoundException) })]
        public async Task ExecuteAsync(Guid taskId, CancellationToken cancellationToken)
        {

            var task = await _taskRepository.GetByPublicId(taskId, cancellationToken) ?? throw new NotFoundException("Task not found.");
            if (task.TaskStatus != Domain.Enums.TaskStatus.Completed)
            {
                _logger.LogInformation($"Task ID: {taskId} is not completed. Current status: {task.TaskStatus}. No notification will be sent.");
                return;
            }

            _logger.LogInformation($"Scheduling task notification for Task ID: {taskId}.");

            // since jop just log info
            // Checking the task status provides only limited protection against duplicate processing.
            // It does not guarantee idempotency because the job may execute multiple times,
            // and structured logs can also be written more than once.

            // If this job creates a notification record in the database,
            // check whether the notification already exists before creating it.
            // To prevent duplicates reliably, enforce a unique constraint in the database
            // based on the business rules, such as TaskId and NotificationType.

            // If this job sends emails through an external service, retries may cause duplicate emails.
            // The Outbox Pattern helps ensure that notification events are not lost between
            // the database transaction and background processing.
            // To prevent duplicate email submissions, use a stable idempotency key,
            // such as the Outbox message ID, if the email provider supports it.
            // Reuse the same key across retries and follow the provider's idempotency guarantees.
            // Note that this does not guarantee exactly-once delivery to the recipient's inbox.



        }
    }
}
