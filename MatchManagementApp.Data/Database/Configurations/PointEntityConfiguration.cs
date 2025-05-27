using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class PointEntityConfiguration : IEntityTypeConfiguration<PointEntity>
{
    public void Configure(EntityTypeBuilder<PointEntity> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.WinnerLabel)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(p => p.WinningMethod)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.NrShots)
            .IsRequired();

        builder.HasOne(p => p.Match)
        .WithMany(m => m.Points)
        .HasForeignKey(p => p.MatchId)
        .OnDelete(DeleteBehavior.Cascade);

    }
}
