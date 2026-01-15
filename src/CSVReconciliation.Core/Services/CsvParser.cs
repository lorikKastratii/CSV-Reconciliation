using CSVReconciliation.Core.Models;

namespace CSVReconciliation.Core.Services;

public class CsvParser
{
    private char _delimiter;
    private bool _hasHeader;

    public CsvParser(char delimiter = ',', bool hasHeader = true)
    {
        _delimiter = delimiter;
        _hasHeader = hasHeader;
    }

    public List<CsvRecord> Parse(string filePath, List<string> errors)
    {
        var records = new List<CsvRecord>();
        var lines = File.ReadAllLines(filePath);

        if (lines.Length == 0)
            return records;

        string[] headers = null;
        int startIndex = 0;

        if (_hasHeader)
        {
            headers = lines[0].Split(_delimiter);
            startIndex = 1;
        }

        for (int i = startIndex; i < lines.Length; i++)
        {
            var line = lines[i];
            if (string.IsNullOrWhiteSpace(line))
                continue;

            try
            {
                var values = line.Split(_delimiter);
                var record = new CsvRecord
                {
                    LineNumber = i + 1,
                    SourceFile = filePath
                };

                if (headers != null)
                {
                    for (int j = 0; j < headers.Length; j++)
                    {
                        record.Fields[headers[j]] = j < values.Length ? values[j] : "";
                    }
                }
                else
                {
                    for (int j = 0; j < values.Length; j++)
                    {
                        record.Fields["Column" + (j + 1)] = values[j];
                    }
                }

                records.Add(record);
            }
            catch (Exception ex)
            {
                errors.Add($"Line {i + 1}: {ex.Message}");
            }
        }

        return records;
    }

    public string[] GetHeaders(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        if (lines.Length == 0)
            return new string[0];
        return lines[0].Split(_delimiter);
    }
}
