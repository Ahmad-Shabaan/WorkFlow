using Domain.Entities;
using Infrastructure.Persistence.Entities;
using Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Infrastructure.Persistence.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ConfigureBase();
            builder.HasOne<ApplicationUser>()
                .WithOne()
                .HasForeignKey<Project>(p => p.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
