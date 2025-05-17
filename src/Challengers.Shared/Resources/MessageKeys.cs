using System.Numerics;

namespace Challengers.Shared.Resources;

public static class MessageKeys
{
    public const string InvalidGender = nameof(InvalidGender);
    public const string Gender_Male = nameof(Gender_Male);
    public const string Gender_Female = nameof(Gender_Female);

    public const string NameRequired = nameof(NameRequired);
    public const string SurnameRequired = nameof(SurnameRequired);
    public const string NameTooLong = nameof(NameTooLong);
    public const string SurnameTooLong = nameof(SurnameTooLong);
    public const string PlayerIdRequired = nameof(PlayerIdRequired);
    public const string SkillOutOfRange = nameof(SkillOutOfRange);
    public const string StrengthOutOfRange = nameof(StrengthOutOfRange);
    public const string SpeedOutOfRange = nameof(SpeedOutOfRange);
    public const string ReactionOutOfRange = nameof(ReactionOutOfRange);

    public const string MatchPlayersRequired = nameof(MatchPlayersRequired);
    public const string MatchSamePlayer = nameof(MatchSamePlayer);
    public const string MatchAlreadySimulated = nameof(MatchAlreadySimulated);

    public const string TournamentNameRequired = nameof(TournamentNameRequired);
    public const string TournamentGenderRequired = nameof(TournamentGenderRequired);
    public const string TournamentAlreadyCompleted = nameof(TournamentAlreadyCompleted);
    public const string TournamentAlreadyExists = nameof(TournamentAlreadyExists);
    public const string TournamentInvalidPlayerCount = nameof(TournamentInvalidPlayerCount);
    public const string TournamentCreatedSuccessfully = nameof(TournamentCreatedSuccessfully);
    public const string TournamentNotFound = nameof(TournamentNotFound);
    public const string DuplicatedPlayerInTournament = nameof(DuplicatedPlayerInTournament);

    public const string FemaleScoreExplanation = nameof(FemaleScoreExplanation);
    public const string MaleScoreExplanation = nameof(MaleScoreExplanation);

    public const string FormatError = nameof(FormatError);
    public const string PlayerIdNotFound = nameof(PlayerIdNotFound);

    public const string ValidationFailedMessage = nameof(ValidationFailedMessage);
    public const string NotFoundMessage = nameof(NotFoundMessage);
    public const string InternalServerErrorMessage = nameof(InternalServerErrorMessage);
    public const string InvalidPlayerData = nameof(InvalidPlayerData);

    public const string StrengthNotAllowedForFemale = nameof(StrengthNotAllowedForFemale);
    public const string SpeedNotAllowedForFemale = nameof(SpeedNotAllowedForFemale);
    public const string StrengthRequiredForMale = nameof(StrengthRequiredForMale);
    public const string SpeedRequiredForMale = nameof(SpeedRequiredForMale);
    public const string MixedGenderAttributes = nameof(MixedGenderAttributes);
    public const string ReactionTimeRequired = nameof(ReactionTimeRequired);
    public const string ReactionTimeNotAllowedForMale = nameof(ReactionTimeNotAllowedForMale);
    public const string TournamentNameTooLong = nameof(TournamentNameTooLong);
    public const string TournamentPlayersEmpty = nameof(TournamentPlayersEmpty);
    public const string PlayerReferenceInvalid = nameof(PlayerReferenceInvalid);
    public const string SkillRequired = nameof(SkillRequired);
    public const string TournamentPlayersGenderMismatch = nameof(TournamentPlayersGenderMismatch);
}
