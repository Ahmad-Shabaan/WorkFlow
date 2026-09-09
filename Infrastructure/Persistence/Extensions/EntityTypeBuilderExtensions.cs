using Domain.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Infrastructure.Persistence.Extensions
{
    public static class EntityTypeBuilderExtensions
    {
        public static void ConfigureBase<TEntitiy>(this EntityTypeBuilder<TEntitiy> entityTypeBuilder) where TEntitiy : BaseEntity
        {
            entityTypeBuilder.HasIndex(e => e.PublicId).IsUnique();
        }
    }
}
