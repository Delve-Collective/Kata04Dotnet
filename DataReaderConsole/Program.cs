namespace DataReaderConsole;

internal class Program
{
    static async Task Main(string[] args)
    {
        var parquetService = new ParquetService();
        var weatherService = new WeatherService(parquetService);
        var footballService = new FootballService(parquetService);

        Console.WriteLine("Select data type to analyze:");
        Console.WriteLine("1 - Weather");
        Console.WriteLine("2 - Football");
        Console.Write("Enter choice: ");
        var choice = Console.ReadLine()?.Trim();

        string result;

        switch (choice)
        {
            case "1":
            case "weather":
                var weatherPath = Path.Combine("..", "..", "..", "..", "Kata04Data", "weather.parquet");
                var day = await weatherService.RetrieveDayWithSmallestTempSpreadAsync(weatherPath);
                result = $"Day with smallest temperature spread: {day}";
                break;

            case "2":
            case "football":
                var footballPath = Path.Combine("..", "..", "..", "..", "Kata04Data", "football.parquet");
                var team = await footballService.GetTeamWithSmallestGoalDifferenceAsync(footballPath);
                result = $"Team with smallest goal difference: {team}";
                break;

            default:
                result = "Invalid selection. Please choose 1 or 2.";
                break;
        }

        Console.WriteLine();
        Console.WriteLine(result);
    }
}