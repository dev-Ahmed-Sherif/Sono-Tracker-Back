using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SonoTracker.Domain.Entities;

namespace SonoTracker.Infrastructure.Configuration
{
    [ExcludeFromCodeCoverage]
    public class TestConfig : BaseConfig<Test, Guid>
    {
        public override void Configure(EntityTypeBuilder<Test> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.NameEn).HasMaxLength(350);
            builder.Property(e => e.NameAr).HasMaxLength(350);
        }
    }
}
