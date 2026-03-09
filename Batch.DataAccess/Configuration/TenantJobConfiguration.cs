using Batch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Batch.DataAccess.Configuration;

internal sealed class TenantJobConfiguration : IEntityTypeConfiguration<TenantJob>
{
    public void Configure(EntityTypeBuilder<TenantJob> builder)
    {
        builder.ToTable("TBJ_TENANT_JOB");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("TBJ_ID");
        builder.Property(x => x.TenantId).HasColumnName("TBJ_TEN_ID").IsRequired();
        builder.Property(x => x.BatchJobId).HasColumnName("TBJ_BAJ_ID").IsRequired();
        builder.Property(x => x.IsEnabled).HasColumnName("TBJ_IS_ENABLED").IsRequired();
        builder.Property(x => x.ParameterJson).HasColumnName("TBJ_PARAM_JSON").HasColumnType("nvarchar(max)");
        builder.Property(x => x.Priority).HasColumnName("TBJ_PRIORITY");
        builder.Property(x => x.UpdatedBy).HasColumnName("TBJ_UPDATED_BY").HasMaxLength(200);
        builder.Property(x => x.UpdatedUtc).HasColumnName("TBJ_UPDATED_UTC").HasColumnType("datetime2(3)").IsRequired();
        builder.HasIndex(x => new { x.TenantId, x.BatchJobId }).HasDatabaseName("UX_TBJ_TEN_ID_BAJ_ID").IsUnique();
    }
}
