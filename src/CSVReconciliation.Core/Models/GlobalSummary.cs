namespace CSVReconciliation.Core.Models;

public class GlobalSummary
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public TimeSpan TotalProcessingTime { get; set; }
    public int TotalFilePairs { get; set; }
    public int TotalRecordsInA { get; set; }
    public int TotalRecordsInB { get; set; }
    public int TotalMatched { get; set; }
    public int TotalOnlyInA { get; set; }
    public int TotalOnlyInB { get; set; }
    public int TotalErrors { get; set; }
    public List<FilePairSummary> FilePairSummaries { get; set; } = new();
    public List<string> MissingFiles { get; set; } = new();
}

public class FilePairSummary
{
    public string FileA { get; set; }
    public string FileB { get; set; }
    public int TotalInA { get; set; }
    public int TotalInB { get; set; }
    public int Matched { get; set; }
    public int OnlyInA { get; set; }
    public int OnlyInB { get; set; }
    public int Errors { get; set; }
    public double ProcessingTimeMs { get; set; }
}
