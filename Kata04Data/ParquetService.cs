namespace Kata04Data;

public interface IParquetService
{
    Task<List<T>> ReadAsync<T>(string parquetPath, Func<Dictionary<string, object>, T> projector,
        params string[] requiredFields);
}

public class ParquetService : IParquetService
{
    public ParquetService()
    {
        
    }

    public async Task<List<T>> ReadAsync<T>(string parquetPath, Func<Dictionary<string, object>, T> projector, params string[] requiredFields)
    {
        await using var stream = File.OpenRead(parquetPath);
        using var reader = await ParquetReader.CreateAsync(stream);
        var groupReader = reader.OpenRowGroupReader(0);
        var schema = reader.Schema.GetDataFields();

        var fields = requiredFields.ToDictionary(
            name => name,
            name => schema.First(f => f.Name == name)
        );

        var columnData = new Dictionary<string, IList>();
        foreach (var (name, field) in fields)
        {
            var column = await groupReader.ReadColumnAsync(field);
            columnData[name] = column.Data;
        }

        var rowCount = columnData.Values.First().Count;
        var records = new List<T>(rowCount);

        for (var i = 0; i < rowCount; i++)
        {
            var row = columnData.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value[i]
            );

            records.Add(projector(row));
        }

        return records;
    }
}