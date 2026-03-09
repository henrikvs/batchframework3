using Batch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Batch.DataAccess.Configuration;

internal sealed class BatchScheduleConfiguration : IEntityTypeConfiguration<BatchSchedule>
{
    public void Configure(EntityTypeBuilder<BatchSchedule> builder)
    {
        builder.ToTable("BSC_BATCH_SCHEDULE");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("BSC_ID");
        builder.Property(x => x.BatchJobId).HasColumnName("BSC_BAJ_ID").IsRequired();
        builder.Property(x => x.Name).HasColumnName("BSC_NAME").HasMaxLength(150).IsRequired();
        builder.Property(x => x.ScheduleType).HasColumnName("BSC_SCHEDULE_TYPE").HasMaxLength(50).HasConversion<string>().IsRequired();
        builder.Property(x => x.TimeZone).HasColumnName("BSC_TIME_ZONE").HasMaxLength(64).IsRequired();
        builder.Property(x => x.ConfigJson).HasColumnName("BSC_CONFIG_JSON").HasColumnType("nvarchar(max)").IsRequired();
        builder.Property(x => x.RecheckEverySeconds).HasColumnName("BSC_RECHECK_EVERY_SEC");
        builder.Property(x => x.LatestStartLocal).HasColumnName("BSC_LATEST_START_LOCAL").HasColumnType("time(0)");
        builder.Property(x => x.MisfirePolicy).HasColumnName("BSC_MISFIRE_POLICY").HasMaxLength(50).HasConversion<string>().IsRequired();
        builder.Property(x => x.IsEnabled).HasColumnName("BSC_ENABLED").IsRequired();
        builder.Property(x => x.MaxParallelGroup).HasColumnName("BSC_MAX_PARALLEL_GROUP");
        builder.Property(x => x.NextFireUtc).HasColumnName("BSC_NEXT_FIRE_UTC").HasColumnType("datetime2(3)");
        builder.Property(x => x.LastSlotKey).HasColumnName("BSC_LAST_SLOT_KEY").HasMaxLength(200);
        builder.Property(x => x.UpdatedBy).HasColumnName("BSC_UPDATED_BY").HasMaxLength(200);
        builder.Property(x => x.UpdatedUtc).HasColumnName("BSC_UPDATED_UTC").HasColumnType("datetime2(3)").IsRequired();
        builder.HasIndex(x => x.BatchJobId).HasDatabaseName("IX_BSC_BAJ_ID");
        builder.HasIndex(x => new { x.IsEnabled, x.NextFireUtc }).HasDatabaseName("IX_BSC_ENABLED_NEXT_FIRE_UTC");
    }
}
