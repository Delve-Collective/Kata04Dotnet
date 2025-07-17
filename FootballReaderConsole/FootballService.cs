using Parquet.Schema;

namespace FootballReaderConsole;

public interface IFootballService
{
    Task<string> GetTeamWithSmallestGoalDifferenceAsync(string parquetPath);
}

public class FootballService : IFootballService
{
    public async Task<string> GetTeamWithSmallestGoalDifferenceAsync(string parquetPath)
    {
        var records = await ReadFootballRecordsAsync(parquetPath);

        return records
            .OrderBy(r => Math.Abs(r.GoalsFor - r.GoalsAgainst))
            .First()
            .Team;
    }

    private async Task<List<FootballRecord>> ReadFootballRecordsAsync(string parquetPath)
    {
        await using var stream = File.OpenRead(parquetPath);
        using var reader = await ParquetReader.CreateAsync(stream);
        var groupReader = reader.OpenRowGroupReader(0);
        var schema = reader.Schema.GetDataFields();

        var teamField = schema.First(f => f.Name == "Team");
        var goalsForField = schema.First(f => f.Name == "F");
        var goalsAgainstField = schema.First(f => f.Name == "A");

        var teamCol = (string[])(await groupReader.ReadColumnAsync(teamField)).Data;
        var goalsForCol = (double?[])(await groupReader.ReadColumnAsync(goalsForField)).Data;
        var goalsAgainstCol = (double?[])(await groupReader.ReadColumnAsync(goalsAgainstField)).Data;


        return Enumerable.Range(0, teamCol.Length)
            .Select(i => new FootballRecord(
                teamCol[i].Trim(),
                (int)(goalsForCol[i] ?? 0),
                (int)(goalsAgainstCol[i] ?? 0)
            ))
            .ToList();
    }
}

public record FootballRecord(string Team, int GoalsFor, int GoalsAgainst);