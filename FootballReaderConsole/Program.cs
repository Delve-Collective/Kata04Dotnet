namespace FootballReaderConsole;

internal class Program
{
    static async Task Main(string[] args)
    {
        var service = new FootballService();

        var path = Path.Combine("..", "..", "..", "..", "Kata04Data", "football.parquet");

        try
        {
            var result = await service.GetTeamWithSmallestGoalDifferenceAsync(path);
            Console.WriteLine($"Day with smallest temperature spread: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}

