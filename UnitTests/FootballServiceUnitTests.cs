namespace UnitTests;

public class FootballServiceUnitTests
{
    [Fact]
    public async Task ShouldGetTeamWithSmallestGoalDifferenceAsync()
    {
        // Arrange
        var parquetService = Substitute.For<IParquetService>();

        var testRecords = new List<FootballRecord>
        {
            new("Arsenal", 80, 40),
            new("Aston_Villa", 50, 49),
            new("Leeds", 60, 59)
        };

        parquetService
            .ReadAsync(
                Arg.Any<string>(),
                Arg.Any<Func<Dictionary<string, object>, FootballRecord>>(),
                Arg.Any<string[]>())
            .Returns(testRecords);

        var service = new FootballService(parquetService);

        // Act
        var result = await service.GetTeamWithSmallestGoalDifferenceAsync("path");

        // Assert
        result.Should().Be("Aston_Villa");
    }
}