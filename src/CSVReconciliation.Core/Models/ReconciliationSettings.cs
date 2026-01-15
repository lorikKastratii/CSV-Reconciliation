namespace CSVReconciliation.Core.Models;

public class ReconciliationSettings
{
    public string FolderA { get; set; }
    public string FolderB { get; set; }
    public string OutputFolder { get; set; }
    public string ConfigPath { get; set; }
    public int MaxThreads { get; set; } = Environment.ProcessorCount;
    public char Delimiter { get; set; } = ',';
    public bool HasHeader { get; set; } = true;
    public bool CompareAllFiles { get; set; }
}
