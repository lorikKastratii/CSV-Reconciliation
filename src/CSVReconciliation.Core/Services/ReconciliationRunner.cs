using System.Collections.Concurrent;
using CSVReconciliation.Core.Models;

namespace CSVReconciliation.Core.Services;

public class ReconciliationRunner
{
    private ReconciliationSettings _settings;
    private MatchingConfig _matchingConfig;

    public ReconciliationRunner(ReconciliationSettings settings, MatchingConfig matchingConfig)
    {
        _settings = settings;
        _matchingConfig = matchingConfig;
    }

    public GlobalSummary Run()
    {
        var summary = new GlobalSummary();
        summary.StartTime = DateTime.Now;

        var finder = new FilePairFinder();
        var pairs = finder.Find(_settings.FolderA, _settings.FolderB);

        summary.TotalFilePairs = pairs.Count;

        var results = new ConcurrentBag<FilePairResult>();

        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = _settings.MaxThreads
        };

        Parallel.ForEach(pairs, options, pair =>
        {
            var parser = new CsvParser(_settings.Delimiter, _settings.HasHeader);
            var matcher = new RecordMatcher(_matchingConfig);
            var reconciler = new Reconciler(parser, matcher);

            var result = reconciler.Compare(pair);
            results.Add(result);

            Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}] Processed: {pair.BaseName}");
        });

        foreach (var result in results)
        {
            if (result.IsMissingFile)
            {
                summary.MissingFiles.Add(result.MissingFileMessage);
                continue;
            }

            summary.TotalRecordsInA += result.TotalInA;
            summary.TotalRecordsInB += result.TotalInB;
            summary.TotalMatched += result.MatchedCount;
            summary.TotalOnlyInA += result.OnlyInACount;
            summary.TotalOnlyInB += result.OnlyInBCount;
            summary.TotalErrors += result.ErrorCount;

            var fileSummary = new FilePairSummary();
            fileSummary.FileA = result.FileNameA;
            fileSummary.FileB = result.FileNameB;
            fileSummary.TotalInA = result.TotalInA;
            fileSummary.TotalInB = result.TotalInB;
            fileSummary.Matched = result.MatchedCount;
            fileSummary.OnlyInA = result.OnlyInACount;
            fileSummary.OnlyInB = result.OnlyInBCount;
            fileSummary.Errors = result.ErrorCount;
            fileSummary.ProcessingTimeMs = result.ProcessingTime.TotalMilliseconds;

            summary.FilePairSummaries.Add(fileSummary);
        }

        summary.EndTime = DateTime.Now;
        summary.TotalProcessingTime = summary.EndTime - summary.StartTime;

        return summary;
    }
}
