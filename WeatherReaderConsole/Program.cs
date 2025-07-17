namespace WeatherReaderConsole;

internal class Program
{
    static async Task Main(string[] args)
    {
        var service = new WeatherService();

        var path = Path.Combine("..", "..", "..", "..", "Kata04Data", "weather.parquet");

        try
        {
            var result = await service.RetrieveDayWithSmallestTempSpreadAsync(path);
            Console.WriteLine($"Day with smallest temperature spread: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
