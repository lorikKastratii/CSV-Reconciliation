using CSVReconciliation.Core.Models;

namespace CSVReconciliation.Tests.Builders;

public class ReconciliationTestBuilder
{
    private string _tempDir;
    private string _fileAName = "test.csv";
    private string _fileBName = "test.csv";
    private List<string> _linesA = new();
    private List<string> _linesB = new();
    private List<string> _matchingFields = new() { "Id" };
    private bool _caseSensitive = false;
    private bool _trim = true;
    private char _delimiter = ',';

    public ReconciliationTestBuilder()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_tempDir);
    }

    public ReconciliationTestBuilder WithFileNames(string fileA, string fileB)
    {
        _fileAName = fileA;
        _fileBName = fileB;
        return this;
    }

    public ReconciliationTestBuilder WithFileAData(params string[] lines)
    {
        _linesA = lines.ToList();
        return this;
    }

    public ReconciliationTestBuilder WithFileBData(params string[] lines)
    {
        _linesB = lines.ToList();
        return this;
    }

    public ReconciliationTestBuilder WithMatchingFields(params string[] fields)
    {
        _matchingFields = fields.ToList();
        return this;
    }

    public ReconciliationTestBuilder WithCaseSensitive(bool caseSensitive)
    {
        _caseSensitive = caseSensitive;
        return this;
    }

    public ReconciliationTestBuilder WithTrim(bool trim)
    {
        _trim = trim;
        return this;
    }

    public ReconciliationTestBuilder WithDelimiter(char delimiter)
    {
        _delimiter = delimiter;
        return this;
    }

    public ReconciliationTestContext Build()
    {
        var folderA = Path.Combine(_tempDir, "FolderA");
        var folderB = Path.Combine(_tempDir, "FolderB");
        Directory.CreateDirectory(folderA);
        Directory.CreateDirectory(folderB);

        var fileA = Path.Combine(folderA, _fileAName);
        var fileB = Path.Combine(folderB, _fileBName);

        if (_linesA.Count > 0)
        {
            File.WriteAllLines(fileA, _linesA);
        }

        if (_linesB.Count > 0)
        {
            File.WriteAllLines(fileB, _linesB);
        }

        var matchingConfig = new MatchingConfig
        {
            MatchingFields = _matchingFields,
            CaseSensitive = _caseSensitive,
            Trim = _trim
        };

        var filePair = new FilePair
        {
            FileA = fileA,
            FileB = fileB,
            FileAExists = File.Exists(fileA),
            FileBExists = File.Exists(fileB),
            BaseName = Path.GetFileNameWithoutExtension(_fileAName)
        };

        return new ReconciliationTestContext
        {
            TempDir = _tempDir,
            FileA = fileA,
            FileB = fileB,
            MatchingConfig = matchingConfig,
            FilePair = filePair,
            Delimiter = _delimiter
        };
    }
}

public class ReconciliationTestContext : IDisposable
{
    public string TempDir { get; set; } = string.Empty;
    public string FileA { get; set; } = string.Empty;
    public string FileB { get; set; } = string.Empty;
    public MatchingConfig MatchingConfig { get; set; } = new MatchingConfig();
    public FilePair FilePair { get; set; } = new FilePair();
    public char Delimiter { get; set; }

    public void Dispose()
    {
        if (Directory.Exists(TempDir))
        {
            try
            {
                Directory.Delete(TempDir, true);
            }
            catch
            {
            }
        }
    }
}
