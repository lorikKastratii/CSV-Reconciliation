using System.Collections.Concurrent;
using CSVReconciliation.Core.Models;

namespace CSVReconciliation.Core.Services;

public class ReconciliationRunner
{
    private ReconciliationSettings _settings;
    private MatchingConfig _matchingConfig;
    private AppLogger _logger;
    private ResultWriter _writer;

    public ReconciliationRunner(ReconciliationSettings settings, MatchingConfig matchingConfig)
    {
        _settings = settings;
        _matchingConfig = matchingConfig;
        _logger = new AppLogger(settings.OutputFolder);
        _writer = new ResultWriter(settings.OutputFolder, settings.Delimiter);
    }

    public GlobalSummary Run()
    {
        var summary = new GlobalSummary();
        summary.StartTime = DateTime.Now;

        _logger.Info("Starting reconciliation");
        _logger.Info($"FolderA: {_settings.FolderA}");
        _logger.Info($"FolderB: {_settings.FolderB}");
        _logger.Info($"Max threads: {_settings.MaxThreads}");

        var finder = new FilePairFinder();
        var pairs = finder.Find(_settings.FolderA, _settings.FolderB);

        summary.TotalFilePairs = pairs.Count;
        _logger.Info($"Found {pairs.Count} file pairs to process");

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

            _logger.Info($"[Thread {Thread.CurrentThread.ManagedThreadId}] Processing: {pair.BaseName}");

            var result = reconciler.Compare(pair);
            results.Add(result);

            _writer.WriteFilePairResult(result, pair.BaseName);

            _logger.Info($"[Thread {Thread.CurrentThread.ManagedThreadId}] Completed: {pair.BaseName} in {result.ProcessingTime.TotalMilliseconds}ms");
        });

        foreach (var result in results)
        {
            if (result.IsMissingFile)
            {
                summary.MissingFiles.Add(result.MissingFileMessage);
                _logger.Warning(result.MissingFileMessage);
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

        _writer.WriteGlobalSummary(summary);
        _logger.Info($"Reconciliation completed in {summary.TotalProcessingTime.TotalMilliseconds}ms");

        return summary;
    }
}
