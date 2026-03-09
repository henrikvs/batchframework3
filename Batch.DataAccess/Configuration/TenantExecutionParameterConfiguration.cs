using Batch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Batch.DataAccess.Configuration;

internal sealed class TenantExecutionParameterConfiguration : IEntityTypeConfiguration<TenantExecutionParameter>
{
    public void Configure(EntityTypeBuilder<TenantExecutionParameter> builder)
    {
        builder.ToTable("TEP_TENANT_EXECUTION_PARAMETER");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("TEP_ID");
        builder.Property(x => x.TenantId).HasColumnName("TEP_TEN_ID").IsRequired();
        builder.Property(x => x.Key).HasColumnName("TEP_KEY").HasMaxLength(100).IsRequired();
        builder.Property(x => x.Value).HasColumnName("TEP_VALUE").HasMaxLength(2000).IsRequired();
        builder.Property(x => x.UpdatedBy).HasColumnName("TEP_UPDATED_BY").HasMaxLength(200);
        builder.Property(x => x.UpdatedUtc).HasColumnName("TEP_UPDATED_UTC").HasColumnType("datetime2(3)").IsRequired();
        builder.HasIndex(x => new { x.TenantId, x.Key }).HasDatabaseName("UX_TEP_TEN_ID_KEY").IsUnique();
        builder.HasIndex(x => x.TenantId).HasDatabaseName("IX_TEP_TEN_ID");
    }
}
