using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SonoTracker.Domain.Entities.Base;
using System.Diagnostics.CodeAnalysis;

namespace SonoTracker.Infrastructure.Configuration
{
    [ExcludeFromCodeCoverage]
    public class BaseConfig<TEntity , TId> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity<TId> where TId : struct
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(e => e.CreatedByEmployeeEn).HasMaxLength(500);
            builder.Property(e => e.CreatedByEmployeeAr).HasMaxLength(500);
            builder.Property(e => e.ModifiedByEmployeeEn).HasMaxLength(500);
            builder.Property(e => e.ModifiedByEmployeeAr).HasMaxLength(500);
            builder.Property(e => e.CreatedById).HasMaxLength(50);
            builder.Property(e => e.ModifiedById).HasMaxLength(50);
            builder.Property(e => e.CreatedByEmployeeId).HasMaxLength(50);
            builder.Property(e => e.ModifiedByEmployeeId).HasMaxLength(50);
            builder.Property(e => e.IpAddress).HasMaxLength(50);
        }
    }
}
