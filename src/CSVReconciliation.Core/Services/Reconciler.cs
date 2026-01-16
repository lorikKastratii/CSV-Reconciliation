using System.Diagnostics;
using CSVReconciliation.Core.Models;

namespace CSVReconciliation.Core.Services;

public class Reconciler
{
    private CsvParser _parser;
    private RecordMatcher _matcher;

    public Reconciler(CsvParser parser, RecordMatcher matcher)
    {
        _parser = parser;
        _matcher = matcher;
    }

    public FilePairResult Compare(FilePair pair)
    {
        var stopwatch = Stopwatch.StartNew();
        var result = new FilePairResult();
        result.FileNameA = pair.FileA;
        result.FileNameB = pair.FileB;

        if (!pair.FileAExists)
        {
            result.IsMissingFile = true;
            result.MissingFileMessage = "File missing in FolderA: " + pair.FileA;
            stopwatch.Stop();
            result.ProcessingTime = stopwatch.Elapsed;
            return result;
        }

        if (!pair.FileBExists)
        {
            result.IsMissingFile = true;
            result.MissingFileMessage = "File missing in FolderB: " + pair.FileB;
            stopwatch.Stop();
            result.ProcessingTime = stopwatch.Elapsed;
            return result;
        }

        var errorsA = new List<string>();
        var errorsB = new List<string>();

        var recordsA = _parser.Parse(pair.FileA, errorsA);
        var recordsB = _parser.Parse(pair.FileB, errorsB);

        result.TotalInA = recordsA.Count;
        result.TotalInB = recordsB.Count;
        result.Errors.AddRange(errorsA);
        result.Errors.AddRange(errorsB);
        result.ErrorCount = result.Errors.Count;

        var dictA = new Dictionary<string, CsvRecord>();
        foreach (var record in recordsA)
        {
            var key = _matcher.GetKey(record);
            dictA[key] = record;
        }

        var matchedKeys = new HashSet<string>();

        foreach (var record in recordsB)
        {
            var key = _matcher.GetKey(record);

            if (dictA.ContainsKey(key))
            {
                result.MatchedRecords.Add(record);
                matchedKeys.Add(key);
            }
            else
            {
                result.OnlyInB.Add(record);
            }
        }

        foreach (var record in recordsA)
        {
            var key = _matcher.GetKey(record);
            if (!matchedKeys.Contains(key))
            {
                result.OnlyInA.Add(record);
            }
        }

        result.MatchedCount = result.MatchedRecords.Count;
        result.OnlyInACount = result.OnlyInA.Count;
        result.OnlyInBCount = result.OnlyInB.Count;

        stopwatch.Stop();
        result.ProcessingTime = stopwatch.Elapsed;

        return result;
    }
}
