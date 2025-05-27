using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class MatchEntityConfiguration : IEntityTypeConfiguration<MatchEntity>
{
    public void Configure(EntityTypeBuilder<MatchEntity> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.MatchType).IsRequired();
        builder.Property(m => m.FirstOpponentName).IsRequired();
        builder.Property(m => m.NrSets).IsRequired();
        builder.Property(m => m.FinalSetType).IsRequired();
        builder.Property(m => m.GameFormat).IsRequired();
        builder.Property(m => m.Surface).IsRequired();

        builder.Property(m => m.MatchDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(m => m.CreatedByUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
