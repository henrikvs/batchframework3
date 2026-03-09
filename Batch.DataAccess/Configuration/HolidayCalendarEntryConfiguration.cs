using Batch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Batch.DataAccess.Configuration;

internal sealed class HolidayCalendarEntryConfiguration : IEntityTypeConfiguration<HolidayCalendarEntry>
{
    public void Configure(EntityTypeBuilder<HolidayCalendarEntry> builder)
    {
        builder.ToTable("CAL_HOLIDAY");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("CAL_ID");
        builder.Property(x => x.Code).HasColumnName("CAL_CODE").HasMaxLength(20).IsRequired();
        builder.Property(x => x.Date).HasColumnName("CAL_DATE").HasColumnType("date").IsRequired();
        builder.Property(x => x.Name).HasColumnName("CAL_NAME").HasMaxLength(200).IsRequired();
        builder.Property(x => x.CreatedUtc).HasColumnName("CAL_CREATED_UTC").HasColumnType("datetime2(3)").IsRequired();
        builder.HasIndex(x => new { x.Code, x.Date }).HasDatabaseName("UX_CAL_CODE_DATE").IsUnique();
    }
}
