using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PointEntityConfiguration : IEntityTypeConfiguration<PointEntity>
{
    public void Configure(EntityTypeBuilder<PointEntity> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.WinningMethod)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.NrShots)
            .IsRequired();

        builder.HasOne(p => p.Match)
            .WithMany() // Or .WithMany(m => m.Points) if Match has a Points collection
            .HasForeignKey(p => p.MatchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Winner)
            .WithMany()
            .HasForeignKey(p => p.WinnerId)
            .OnDelete(DeleteBehavior.NoAction); // Avoid multiple cascade paths
    }
}
