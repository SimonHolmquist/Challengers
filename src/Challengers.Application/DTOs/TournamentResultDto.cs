﻿using Challengers.Domain.Enums;

namespace Challengers.Application.DTOs
{
    public record class TournamentResultDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
        public Gender Gender { get; init; } = default!;
        public DateTime CreatedAt { get; init; }
        public string Winner { get; init; } = default!;
    }
}
