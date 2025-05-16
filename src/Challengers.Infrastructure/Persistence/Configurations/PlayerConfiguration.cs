using Challengers.Domain.Constants;
using Challengers.Domain.Entities;
using Challengers.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Challengers.Infrastructure.Persistence.Configurations;

public class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(ValidationRules.PlayerNameMaxLength);

        builder.Property(p => p.Skill)
            .IsRequired();

        builder.HasDiscriminator(p => p.Gender)
            .HasValue<MalePlayer>(Gender.Male)
            .HasValue<FemalePlayer>(Gender.Female);

    }
}
