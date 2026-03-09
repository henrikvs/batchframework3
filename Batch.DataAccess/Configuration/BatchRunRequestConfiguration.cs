using Batch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Batch.DataAccess.Configuration;

internal sealed class BatchRunRequestConfiguration : IEntityTypeConfiguration<BatchRunRequest>
{
    public void Configure(EntityTypeBuilder<BatchRunRequest> builder)
    {
        builder.ToTable("BRQ_BATCH_RUN_REQUEST");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("BRQ_ID");
        builder.Property(x => x.TriggerType).HasColumnName("BRQ_TRIGGER_TYPE").HasMaxLength(50).HasConversion<string>().IsRequired();
        builder.Property(x => x.TenantId).HasColumnName("BRQ_TEN_ID");
        builder.Property(x => x.BatchJobId).HasColumnName("BRQ_BAJ_ID");
        builder.Property(x => x.BatchScheduleId).HasColumnName("BRQ_BSC_ID");
        builder.Property(x => x.SlotKey).HasColumnName("BRQ_SLOT_KEY").HasMaxLength(200);
        builder.Property(x => x.FireAtUtc).HasColumnName("BRQ_FIRE_AT_UTC").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(x => x.IncludePrerequisites).HasColumnName("BRQ_INCLUDE_PREREQS").IsRequired();
        builder.Property(x => x.Mode).HasColumnName("BRQ_MODE").HasMaxLength(50).HasConversion<string>().IsRequired();
        builder.Property(x => x.TargetTenantFilterJson).HasColumnName("BRQ_TARGET_TENANT_FILTER_JSON").HasColumnType("nvarchar(max)");
        builder.Property(x => x.ExecutionContextJson).HasColumnName("BRQ_EXECUTION_CONTEXT_JSON").HasColumnType("nvarchar(max)");
        builder.Property(x => x.ScopeKey).HasColumnName("BRQ_SCOPE_KEY").HasMaxLength(200).IsRequired();
        builder.Property(x => x.RespectDependencies).HasColumnName("BRQ_RESPECT_DEPENDENCIES").IsRequired();
        builder.Property(x => x.RespectGates).HasColumnName("BRQ_RESPECT_GATES").IsRequired();
        builder.Property(x => x.OnlyIfReady).HasColumnName("BRQ_ONLY_IF_READY").IsRequired();
        builder.Property(x => x.ForceGates).HasColumnName("BRQ_FORCE_GATES").IsRequired();
        builder.Property(x => x.Status).HasColumnName("BRQ_STATUS").HasMaxLength(50).HasConversion<string>().IsRequired();
        builder.Property(x => x.Reason).HasColumnName("BRQ_REASON").HasMaxLength(500);
        builder.Property(x => x.CreatedBy).HasColumnName("BRQ_CREATED_BY").HasMaxLength(200).IsRequired();
        builder.Property(x => x.CreatedUtc).HasColumnName("BRQ_CREATED_UTC").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(x => x.UpdatedUtc).HasColumnName("BRQ_UPDATED_UTC").HasColumnType("datetime2(3)").IsRequired();
        builder.HasIndex(x => new { x.BatchScheduleId, x.SlotKey }).HasDatabaseName("UX_BRQ_BSC_ID_SLOT_KEY").IsUnique().HasFilter("[BRQ_BSC_ID] IS NOT NULL");
        builder.HasIndex(x => new { x.Status, x.FireAtUtc }).HasDatabaseName("IX_BRQ_STATUS_FIRE_AT_UTC");
    }
}
