using Domain.Entities;
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Infrastructure.Persistence.Configurations
{
    public class TaskAssignmentConfigruation : IEntityTypeConfiguration<TaskAssignment>
    {
        public void Configure(EntityTypeBuilder<TaskAssignment> builder)
        {
            builder.HasKey(ta => new { ta.TaskId, ta.UserId });

            builder.HasOne(ta => ta.Task)
                .WithMany(t=> t.TaskAssignments)
                .HasForeignKey(ta => ta.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(ta => ta.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
