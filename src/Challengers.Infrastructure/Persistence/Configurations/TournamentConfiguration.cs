using Challengers.Domain.Constants;
using Challengers.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Challengers.Infrastructure.Persistence.Configurations;

public class TournamentConfiguration : IEntityTypeConfiguration<Tournament>
{
    public void Configure(EntityTypeBuilder<Tournament> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(ValidationRules.TournamentNameMaxLength);

        builder.Property(t => t.Gender)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(t => t.CreatedAt)
            .IsRequired()
            .HasConversion<string>();

        builder.HasMany(t => t.Matches)
            .WithOne(m => m.Tournament)
            .HasForeignKey(m => m.TournamentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.Players)
            .WithMany(p => p.Tournaments);

        builder.HasOne(t => t.Winner)
            .WithMany()
            .HasForeignKey(t => t.WinnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(t => t.IsCompleted)
            .HasColumnName("IsCompleted")
            .IsRequired();

    }
}
