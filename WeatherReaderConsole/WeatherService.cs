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
        return 10;
    }
}