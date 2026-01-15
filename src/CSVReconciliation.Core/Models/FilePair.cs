namespace CSVReconciliation.Core.Models;

public class FilePair
{
    public string FileA { get; set; }
    public string FileB { get; set; }
    public string BaseName { get; set; }
    public bool FileAExists { get; set; }
    public bool FileBExists { get; set; }
}
