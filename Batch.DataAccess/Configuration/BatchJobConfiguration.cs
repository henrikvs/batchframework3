using Batch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Batch.DataAccess.Configuration;

internal sealed class BatchJobConfiguration : IEntityTypeConfiguration<BatchJob>
{
    public void Configure(EntityTypeBuilder<BatchJob> builder)
    {
        builder.ToTable("BAJ_BATCH_JOB");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("BAJ_ID");
        builder.Property(x => x.JobName).HasColumnName("BAJ_JOBNAME").HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasColumnName("BAJ_DESCRIPTION").HasMaxLength(500);
        builder.Property(x => x.RunnerType).HasColumnName("BAJ_RUNNER_TYPE").HasMaxLength(50).HasConversion<string>().IsRequired();
        builder.Property(x => x.RunnerProfileId).HasColumnName("BAJ_RUN_ID").IsRequired();
        builder.Property(x => x.CommandTarget).HasColumnName("BAJ_COMMAND_TARGET").HasMaxLength(300);
        builder.Property(x => x.ExtraArgumentsTemplate).HasColumnName("BAJ_EXTRA_ARGS_TEMPLATE").HasMaxLength(1000);
        builder.Property(x => x.DateArgumentMode).HasColumnName("BAJ_DATE_ARGUMENT_MODE").HasMaxLength(50).HasConversion<string>().IsRequired();
        builder.Property(x => x.DateArgumentName).HasColumnName("BAJ_DATE_ARGUMENT_NAME").HasMaxLength(100);
        builder.Property(x => x.DateFormat).HasColumnName("BAJ_DATE_FORMAT").HasMaxLength(50);
        builder.Property(x => x.DateOffsetDays).HasColumnName("BAJ_DATE_OFFSET_DAYS");
        builder.Property(x => x.WorkDirectoryOverride).HasColumnName("BAJ_WORK_DIR_OVERRIDE").HasMaxLength(500);
        builder.Property(x => x.TimeoutSeconds).HasColumnName("BAJ_TIMEOUT_SEC").IsRequired();
        builder.Property(x => x.MutexGroup).HasColumnName("BAJ_MUTEX_GROUP").HasMaxLength(100);
        builder.Property(x => x.MaxParallelGlobal).HasColumnName("BAJ_MAX_PARALLEL_GLOBAL");
        builder.Property(x => x.MaxParallelPerTenant).HasColumnName("BAJ_MAX_PARALLEL_PER_TENANT");
        builder.Property(x => x.EnabledDefault).HasColumnName("BAJ_ENABLED_DEFAULT").IsRequired();
        builder.Property(x => x.CreatedUtc).HasColumnName("BAJ_CREATED_UTC").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(x => x.UpdatedUtc).HasColumnName("BAJ_UPDATED_UTC").HasColumnType("datetime2(3)").IsRequired();
        builder.HasIndex(x => x.JobName).HasDatabaseName("UX_BAJ_JOBNAME").IsUnique();
        builder.HasIndex(x => x.RunnerProfileId).HasDatabaseName("IX_BAJ_RUN_ID");
    }
}
