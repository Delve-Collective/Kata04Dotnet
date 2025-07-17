namespace UnitTests;

public class WeatherServiceUnitTests
{
    [Fact]
    public async Task ShouldRetrieveDayWithSmallestTempSpreadAsync()
    {
        // Arrange
        var service = new WeatherService();
        var path = Path.Combine("..", "..", "..", "..", "Kata04Data", "weather.parquet");

        // Act
        var result = await service.RetrieveDayWithSmallestTempSpreadAsync(path);

        // Assert
        result.Should().Be(14);
    }
}