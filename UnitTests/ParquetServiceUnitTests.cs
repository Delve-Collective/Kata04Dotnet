namespace UnitTests;

public class ParquetServiceUnitTests
{
    [Fact]
    public async Task ReadAsyncShouldReturnCorrectWeatherRecords()
    {
        // Arrange
        var service = new ParquetService();
        var path = Path.Combine("..", "..", "..", "..", "Kata04Data", "weather.parquet");

        // Act
        var result = await service.ReadAsync(
            path,
            row => new WeatherRecord(
                int.Parse(row["Dy"].ToString()!.Replace("*", "").Trim()),
                float.Parse(row["MxT"].ToString()!.Replace("*", "").Trim()),
                float.Parse(row["MnT"].ToString()!.Replace("*", "").Trim())
            ),
            "Dy", "MxT", "MnT"
        );

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCountGreaterThan(0);

        result.First().Day.Should().BeGreaterThan(0);
        result.First().MaxTemp.Should().BeGreaterThan(result.First().MinTemp);
    }
    [Fact]
    public async Task ReadAsyncShouldReturnCorrectFootballRecords()
    {
        // Arrange
        var service = new ParquetService();
        var path = Path.Combine("..", "..", "..", "..", "Kata04Data", "football.parquet");

        // Act
        var result = await service.ReadAsync(
            path,
            row => new FootballRecord(
                row["Team"].ToString()!.Trim(),
                Convert.ToInt32(row["F"]),
                Convert.ToInt32(row["A"])
            ),
            "Team", "F", "A"
        );

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCountGreaterThan(0);

        var first = result.First();
        first.Team.Should().NotBeNullOrWhiteSpace();
        first.GoalsFor.Should().BeGreaterThanOrEqualTo(0);
        first.GoalsAgainst.Should().BeGreaterThanOrEqualTo(0);
    }
}