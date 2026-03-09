using Batch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Batch.DataAccess.Configuration;

internal sealed class TenantJobScheduleBindingConfiguration : IEntityTypeConfiguration<TenantJobScheduleBinding>
{
    public void Configure(EntityTypeBuilder<TenantJobScheduleBinding> builder)
    {
        builder.ToTable("TJS_TENANT_JOB_SCHEDULE");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("TJS_ID");
        builder.Property(x => x.TenantId).HasColumnName("TJS_TEN_ID").IsRequired();
        builder.Property(x => x.BatchJobId).HasColumnName("TJS_BAJ_ID").IsRequired();
        builder.Property(x => x.BatchScheduleId).HasColumnName("TJS_BSC_ID").IsRequired();
        builder.Property(x => x.IsEnabled).HasColumnName("TJS_ENABLED").IsRequired();
        builder.Property(x => x.EffectiveFromUtc).HasColumnName("TJS_EFFECTIVE_FROM_UTC").HasColumnType("datetime2(3)");
        builder.Property(x => x.EffectiveToUtc).HasColumnName("TJS_EFFECTIVE_TO_UTC").HasColumnType("datetime2(3)");
        builder.Property(x => x.UpdatedBy).HasColumnName("TJS_UPDATED_BY").HasMaxLength(200);
        builder.Property(x => x.UpdatedUtc).HasColumnName("TJS_UPDATED_UTC").HasColumnType("datetime2(3)").IsRequired();
        builder.HasIndex(x => new { x.TenantId, x.BatchJobId, x.BatchScheduleId }).HasDatabaseName("UX_TJS_TEN_ID_BAJ_ID_BSC_ID").IsUnique();
    }
}
