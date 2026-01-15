namespace CSVReconciliation.Core.Models;

public class CsvRecord
{
    public Dictionary<string, string> Fields { get; set; } = new();
    public int LineNumber { get; set; }
    public string SourceFile { get; set; }

    public string GetValue(string fieldName)
    {
        return Fields.TryGetValue(fieldName, out var value) ? value : "";
    }
}
