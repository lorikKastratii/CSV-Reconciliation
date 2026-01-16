using System.Text.Json;
using CSVReconciliation.Core.Models;

namespace CSVReconciliation.Core.Services;

public class ResultWriter
{
    private string _outputFolder;
    private char _delimiter;

    public ResultWriter(string outputFolder, char delimiter = ',')
    {
        _outputFolder = outputFolder;
        _delimiter = delimiter;

        if (!Directory.Exists(_outputFolder))
            Directory.CreateDirectory(_outputFolder);
    }

    public void WriteFilePairResult(FilePairResult result, string baseName)
    {
        var folder = Path.Combine(_outputFolder, baseName);
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        var csvWriter = new CsvWriter(_delimiter);

        csvWriter.Write(Path.Combine(folder, "matched.csv"), result.MatchedRecords);
        csvWriter.Write(Path.Combine(folder, "only-in-folderA.csv"), result.OnlyInA);
        csvWriter.Write(Path.Combine(folder, "only-in-folderB.csv"), result.OnlyInB);

        var summary = new
        {
            fileA = result.FileNameA,
            fileB = result.FileNameB,
            totalInA = result.TotalInA,
            totalInB = result.TotalInB,
            matched = result.MatchedCount,
            onlyInA = result.OnlyInACount,
            onlyInB = result.OnlyInBCount,
            errors = result.ErrorCount,
            processingTimeMs = result.ProcessingTime.TotalMilliseconds
        };

        var json = JsonSerializer.Serialize(summary, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(Path.Combine(folder, "reconcile-summary.json"), json);

        if (result.Errors.Count > 0)
        {
            File.WriteAllLines(Path.Combine(folder, "errors.csv"), result.Errors);
        }
    }

    public void WriteGlobalSummary(GlobalSummary summary)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(summary, options);
        File.WriteAllText(Path.Combine(_outputFolder, "global-summary.json"), json);
    }
}
