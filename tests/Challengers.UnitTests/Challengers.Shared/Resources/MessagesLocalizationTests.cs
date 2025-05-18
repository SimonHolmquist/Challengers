using Challengers.Shared.Resources;
using System.Globalization;
using Xunit;
using FluentAssertions;
using Challengers.Shared.Helpers;

namespace Challengers.UnitTests.Challengers.Shared.Resources;

public class MessagesLocalizationTests
{
    [Fact]
    public void Should_Return_English_Message_By_Default()
    {
        var originalCulture = CultureInfo.DefaultThreadCurrentUICulture;
        try
        {
            // Arrange
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

            // Act
            var message = LocalizedMessages.GetMessage(MessageKeys.PlayerIdRequired);

            // Assert
            message.Should().Be("Player ID is required when referencing an existing player.");
        }
        finally
        {
            CultureInfo.DefaultThreadCurrentUICulture = originalCulture;
        }
    }

    [Fact]
    public void Should_Return_Spanish_Message_When_Culture_Is_Es()
    {
        var originalCulture = CultureInfo.DefaultThreadCurrentUICulture;
        try
        {
            // Arrange
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("es-AR");

            // Act
            var message = LocalizedMessages.GetMessage(MessageKeys.PlayerIdRequired);

            // Assert
            message.Should().Be("Se requiere el ID del jugador cuando se hace referencia a un jugador existente.");
        }
        finally
        {
            CultureInfo.DefaultThreadCurrentUICulture = originalCulture;
        }
    }
}
