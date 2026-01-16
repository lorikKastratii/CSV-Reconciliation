using CSVReconciliation.Core.Models;

namespace CSVReconciliation.Core.Services;

public class CsvWriter
{
    private char _delimiter;

    public CsvWriter(char delimiter = ',')
    {
        _delimiter = delimiter;
    }

    public void Write(string filePath, List<CsvRecord> records)
    {
        if (records.Count == 0)
        {
            File.WriteAllText(filePath, "");
            return;
        }

        var headers = records[0].Fields.Keys.ToList();
        var lines = new List<string>();

        lines.Add(string.Join(_delimiter, headers));

        foreach (var record in records)
        {
            var values = new List<string>();
            foreach (var header in headers)
            {
                var value = record.GetValue(header);
                values.Add(value);
            }
            lines.Add(string.Join(_delimiter, values));
        }

        File.WriteAllLines(filePath, lines);
    }
}
