﻿using Challengers.Domain.Constants;
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

        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(PlayerConstants.MaxNameLength);

        builder.Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(PlayerConstants.MaxLastNameLength);

        builder.Property(p => p.Skill)
            .IsRequired();

        builder.HasDiscriminator(p => p.Gender)
            .HasValue<MalePlayer>(Gender.Male)
            .HasValue<FemalePlayer>(Gender.Female);

    }
}
