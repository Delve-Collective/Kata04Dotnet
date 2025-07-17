using Kata04Data;

namespace UnitTests;

public class FootballServiceUnitTests
{
    [Fact]
    public async Task ShouldGetTeamWithSmallestGoalDifferenceAsync()
    {
        // Arrange
        var service = new FootballService();
        var path = Path.Combine("..", "..", "..", "..", "Kata04Data", "football.parquet");

        // Act
        var result = await service.GetTeamWithSmallestGoalDifferenceAsync(path);

        // Assert
        result.Should().Be("Aston_Villa");
    }
}