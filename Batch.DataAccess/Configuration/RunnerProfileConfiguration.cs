using Batch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Batch.DataAccess.Configuration;

internal sealed class RunnerProfileConfiguration : IEntityTypeConfiguration<RunnerProfile>
{
    public void Configure(EntityTypeBuilder<RunnerProfile> builder)
    {
        builder.ToTable("RUN_RUNNER_PROFILE");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("RUN_ID");
        builder.Property(x => x.Name).HasColumnName("RUN_NAME").HasMaxLength(150).IsRequired();
        builder.Property(x => x.Description).HasColumnName("RUN_DESCRIPTION").HasMaxLength(500);
        builder.Property(x => x.RunnerType).HasColumnName("RUN_RUNNER_TYPE").HasMaxLength(50).HasConversion<string>().IsRequired();
        builder.Property(x => x.ProfileMode).HasColumnName("RUN_PROFILE_MODE").HasMaxLength(50).HasConversion<string>().IsRequired();
        builder.Property(x => x.ExecutableTemplate).HasColumnName("RUN_EXECUTABLE_TEMPLATE").HasMaxLength(1000).IsRequired();
        builder.Property(x => x.WorkDirectoryTemplate).HasColumnName("RUN_WORK_DIR_TEMPLATE").HasMaxLength(1000);
        builder.Property(x => x.ArgumentTemplate).HasColumnName("RUN_ARGUMENT_TEMPLATE").HasMaxLength(2000);
        builder.Property(x => x.ArgumentJoiner).HasColumnName("RUN_ARGUMENT_JOINER").HasMaxLength(10);
        builder.Property(x => x.IsEnabled).HasColumnName("RUN_IS_ENABLED").IsRequired();
        builder.Property(x => x.CreatedUtc).HasColumnName("RUN_CREATED_UTC").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(x => x.UpdatedUtc).HasColumnName("RUN_UPDATED_UTC").HasColumnType("datetime2(3)").IsRequired();
        builder.HasIndex(x => x.Name).HasDatabaseName("UX_RUN_NAME").IsUnique();
    }
}
