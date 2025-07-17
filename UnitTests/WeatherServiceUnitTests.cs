namespace UnitTests;

public class WeatherServiceUnitTests
{
    [Fact]
    public async Task ShouldRetrieveDayWithSmallestTempSpreadAsync()
    {
        // Arrange
        var parquetService = Substitute.For<IParquetService>();

        var testRecords = new List<WeatherRecord>
        {
            new(1, 88, 59),
            new(14, 61, 59),
            new(3, 77, 55)
        };

        parquetService
            .ReadAsync(
                Arg.Any<string>(),
                Arg.Any<Func<Dictionary<string, object>, WeatherRecord>>(),
                Arg.Any<string[]>())
            .Returns(testRecords);

        var service = new WeatherService(parquetService);
        var path = "fake-path"; // doesn't matter for mock

        // Act
        var result = await service.RetrieveDayWithSmallestTempSpreadAsync(path);

        // Assert
        result.Should().Be(14);
    }
}