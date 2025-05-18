using Challengers.Application.DTOs;
using Challengers.Domain.Enums;
using FluentValidation;

namespace Challengers.Application.Validators;

public class CreateTournamentRequestDtoValidator : AbstractValidator<CreateTournamentRequestDto>
{
    public CreateTournamentRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(GetMessage(TournamentNameRequired))
            .MaximumLength(TournamentNameMaxLength).WithMessage(FormatMessage(TournamentNameTooLong, TournamentNameMaxLength));

        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage(GetMessage(TournamentGenderRequired))
            .Must(gender => gender == Gender.Male || gender == Gender.Female).WithMessage(InvalidGender);

        RuleFor(x => x.Players)
            .Must(players => players.Select(p => p.Id ?? Guid.NewGuid()).Distinct().Count() == players.Count)
            .WithMessage(GetMessage(DuplicatedPlayerInTournament));

        RuleFor(x => x.Players)
            .NotEmpty().WithMessage(TournamentPlayersEmpty)
            .Must(p => p.Count > 1 && (p.Count & (p.Count - 1)) == 0)
            .WithMessage(GetMessage(TournamentInvalidPlayerCount));

        RuleForEach(x => x.Players).SetValidator(new PlayerReferenceOrCreateValidator());
    }
}
