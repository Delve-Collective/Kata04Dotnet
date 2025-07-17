namespace Kata04Data;

public interface IWeatherService
{
    Task<int> RetrieveDayWithSmallestTempSpreadAsync(string parquetPath);
}

public class WeatherService : IWeatherService
{
    private readonly IParquetService _parquetService;

    public WeatherService(IParquetService parquetService)
    {
        _parquetService = parquetService;
    }

    public async Task<int> RetrieveDayWithSmallestTempSpreadAsync(string parquetPath)
    {
        var records = await _parquetService.ReadAsync(
            parquetPath,
            row => new WeatherRecord(
                int.Parse(row["Dy"]?.ToString()?.Replace("*", "").Trim() ?? "0"),
                float.Parse(row["MxT"]?.ToString()?.Replace("*", "").Trim() ?? "0"),
                float.Parse(row["MnT"]?.ToString()?.Replace("*", "").Trim() ?? "0")
            ),
            "Dy", "MxT", "MnT"
        );

        return records
            .OrderBy(r => Math.Abs(r.MaxTemp - r.MinTemp))
            .First()
            .Day;
    }
}

public record WeatherRecord(int Day, float MaxTemp, float MinTemp);