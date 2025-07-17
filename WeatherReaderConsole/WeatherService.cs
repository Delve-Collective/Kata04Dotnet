namespace WeatherReaderConsole;

public interface IWeatherService
{
    Task<int> RetrieveDayWithSmallestTempSpreadAsync(string parquetPath);
}

public class WeatherService : IWeatherService
{
    public WeatherService()
    {

    }

    public async Task<int> RetrieveDayWithSmallestTempSpreadAsync(string parquetPath)
    {
        var records = await ReadWeatherRecordsAsync(parquetPath);

        return records
            .OrderBy(r => Math.Abs(r.MaxTemp - r.MinTemp))
            .First()
            .Day;
    }

    private async Task<List<WeatherRecord>> ReadWeatherRecordsAsync(string parquetPath)
    {
        await using var stream = File.OpenRead(parquetPath);
        using var reader = await ParquetReader.CreateAsync(stream);
        var groupReader = reader.OpenRowGroupReader(0);
        var schema = reader.Schema.GetDataFields();

        var dayField = schema.First(f => f.Name == "Dy");
        var maxField = schema.First(f => f.Name == "MxT");
        var minField = schema.First(f => f.Name == "MnT");

        var dyCol = (string[])(await groupReader.ReadColumnAsync(dayField)).Data;
        var maxCol = (string[])(await groupReader.ReadColumnAsync(maxField)).Data;
        var minCol = (string[])(await groupReader.ReadColumnAsync(minField)).Data;

        return Enumerable.Range(0, dyCol.Length)
            .Select(i => new WeatherRecord(
                int.Parse(dyCol[i].Replace("*", "").Trim()),
                float.Parse(maxCol[i].Replace("*", "").Trim()),
                float.Parse(minCol[i].Replace("*", "").Trim())
            ))
            .ToList();
    }
}

public record WeatherRecord(int Day, float MaxTemp, float MinTemp);