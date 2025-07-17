namespace Kata04Data;

public interface IFootballService
{
    Task<string> GetTeamWithSmallestGoalDifferenceAsync(string parquetPath);
}

public class FootballService : IFootballService
{
    private readonly IParquetService _parquetService;

    public FootballService(IParquetService parquetService)
    {
        _parquetService = parquetService;
    }

    public async Task<string> GetTeamWithSmallestGoalDifferenceAsync(string parquetPath)
    {
        var records = await _parquetService.ReadAsync(
            parquetPath,
            row => new FootballRecord(
                row["Team"].ToString()!,
                Convert.ToInt32(row["F"]),
                Convert.ToInt32(row["A"])
            ),
            "Team", "F", "A"
        );

        return records
            .OrderBy(r => Math.Abs(r.GoalsFor - r.GoalsAgainst))
            .First()
            .Team;
    }
}

public record FootballRecord(string Team, int GoalsFor, int GoalsAgainst);