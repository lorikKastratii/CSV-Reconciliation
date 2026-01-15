namespace CSVReconciliation.Core.Models;

public class FilePairResult
{
    public string FileNameA { get; set; }
    public string FileNameB { get; set; }
    public int TotalInA { get; set; }
    public int TotalInB { get; set; }
    public int MatchedCount { get; set; }
    public int OnlyInACount { get; set; }
    public int OnlyInBCount { get; set; }
    public int ErrorCount { get; set; }
    public TimeSpan ProcessingTime { get; set; }
    public List<CsvRecord> MatchedRecords { get; set; } = new();
    public List<CsvRecord> OnlyInA { get; set; } = new();
    public List<CsvRecord> OnlyInB { get; set; } = new();
    public List<string> Errors { get; set; } = new();
    public bool IsMissingFile { get; set; }
    public string MissingFileMessage { get; set; }
}
