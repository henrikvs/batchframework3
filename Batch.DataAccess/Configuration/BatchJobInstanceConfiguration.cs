using Batch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Batch.DataAccess.Configuration;

internal sealed class BatchJobInstanceConfiguration : IEntityTypeConfiguration<BatchJobInstance>
{
    public void Configure(EntityTypeBuilder<BatchJobInstance> builder)
    {
        builder.ToTable("BIN_BATCH_JOB_INSTANCE");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("BIN_ID");
        builder.Property(x => x.BatchRunRequestId).HasColumnName("BIN_BRQ_ID").IsRequired();
        builder.Property(x => x.TenantId).HasColumnName("BIN_TEN_ID").IsRequired();
        builder.Property(x => x.BatchJobId).HasColumnName("BIN_BAJ_ID").IsRequired();
        builder.Property(x => x.BatchScheduleId).HasColumnName("BIN_BSC_ID");
        builder.Property(x => x.OccurrenceKey).HasColumnName("BIN_OCCURRENCE_KEY").HasMaxLength(200).IsRequired();
        builder.Property(x => x.ScopeKey).HasColumnName("BIN_SCOPE_KEY").HasMaxLength(200).IsRequired();
        builder.Property(x => x.ExecutionContextJson).HasColumnName("BIN_EXECUTION_CONTEXT_JSON").HasColumnType("nvarchar(max)");
        builder.Property(x => x.PlannedStartUtc).HasColumnName("BIN_PLANNED_START_UTC").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(x => x.Status).HasColumnName("BIN_STATUS").HasMaxLength(50).HasConversion<string>().IsRequired();
        builder.Property(x => x.BlockReason).HasColumnName("BIN_BLOCK_REASON").HasMaxLength(500);
        builder.Property(x => x.AttemptNumber).HasColumnName("BIN_ATTEMPT_NO").IsRequired();
        builder.Property(x => x.NextEvaluationUtc).HasColumnName("BIN_NEXT_EVAL_UTC").HasColumnType("datetime2(3)");
        builder.Property(x => x.CreatedUtc).HasColumnName("BIN_CREATED_UTC").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(x => x.UpdatedUtc).HasColumnName("BIN_UPDATED_UTC").HasColumnType("datetime2(3)").IsRequired();
        builder.HasIndex(x => new { x.TenantId, x.BatchJobId, x.BatchScheduleId, x.OccurrenceKey, x.ScopeKey })
            .HasDatabaseName("UX_BIN_TEN_ID_BAJ_ID_BSC_ID_OCCURRENCE_KEY_SCOPE_KEY")
            .IsUnique();
        builder.HasIndex(x => new { x.Status, x.PlannedStartUtc }).HasDatabaseName("IX_BIN_STATUS_PLANNED_START");
        builder.HasIndex(x => new { x.TenantId, x.Status }).HasDatabaseName("IX_BIN_TEN_ID_STATUS");
        builder.HasIndex(x => new { x.TenantId, x.OccurrenceKey, x.ScopeKey }).HasDatabaseName("IX_BIN_TEN_ID_OCCURRENCE_KEY_SCOPE_KEY");
    }
}
