using Batch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Batch.DataAccess.Configuration;

internal sealed class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("TEN_TENANT");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("TEN_ID");
        builder.Property(x => x.Code).HasColumnName("TEN_CODE").HasMaxLength(50).IsRequired();
        builder.Property(x => x.Name).HasColumnName("TEN_NAME").HasMaxLength(200).IsRequired();
        builder.Property(x => x.Status).HasColumnName("TEN_STATUS").HasMaxLength(50).HasConversion<string>().IsRequired();
        builder.Property(x => x.TimeZone).HasColumnName("TEN_TIME_ZONE").HasMaxLength(64).IsRequired();
        builder.Property(x => x.RootPath).HasColumnName("TEN_ROOT_PATH").HasMaxLength(1000);
        builder.Property(x => x.Tags).HasColumnName("TEN_TAGS").HasMaxLength(500);
        builder.Property(x => x.CreatedUtc).HasColumnName("TEN_CREATED_UTC").HasColumnType("datetime2(3)").IsRequired();
        builder.Property(x => x.UpdatedUtc).HasColumnName("TEN_UPDATED_UTC").HasColumnType("datetime2(3)").IsRequired();
        builder.HasIndex(x => x.Code).HasDatabaseName("UX_TEN_CODE").IsUnique();
    }
}
