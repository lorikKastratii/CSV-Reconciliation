using CSVReconciliation.Core.Services;
using CSVReconciliation.Tests.Builders;

namespace CSVReconciliation.Tests;

public class ReconcilerTests
{
    [Fact]
    public void Compare_AllMatched_ReturnsCorrectCounts()
    {
        using var context = new ReconciliationTestBuilder()
            .WithFileAData(
                "Id,Name",
                "1,John",
                "2,Jane")
            .WithFileBData(
                "Id,Name",
                "1,John",
                "2,Jane")
            .WithMatchingFields("Id")
            .Build();

        var parser = new CsvParser(context.Delimiter, true);
        var matcher = new RecordMatcher(context.MatchingConfig);
        var reconciler = new Reconciler(parser, matcher);

        var result = reconciler.Compare(context.FilePair);

        Assert.Equal(2, result.MatchedCount);
        Assert.Equal(0, result.OnlyInACount);
        Assert.Equal(0, result.OnlyInBCount);
    }

    [Fact]
    public void Compare_SomeUnmatched_ReturnsCorrectCounts()
    {
        using var context = new ReconciliationTestBuilder()
            .WithFileAData(
                "Id,Name",
                "1,John",
                "2,Jane",
                "3,Bob")
            .WithFileBData(
                "Id,Name",
                "1,John",
                "4,Alice")
            .WithMatchingFields("Id")
            .Build();

        var parser = new CsvParser(context.Delimiter, true);
        var matcher = new RecordMatcher(context.MatchingConfig);
        var reconciler = new Reconciler(parser, matcher);

        var result = reconciler.Compare(context.FilePair);

        Assert.Equal(1, result.MatchedCount);
        Assert.Equal(2, result.OnlyInACount);
        Assert.Equal(1, result.OnlyInBCount);
    }

    [Fact]
    public void Compare_MissingFileA_ReturnsMissingFlag()
    {
        var parser = new CsvParser(',', true);
        var config = new Core.Models.MatchingConfig { MatchingFields = new List<string> { "Id" } };
        var matcher = new RecordMatcher(config);
        var reconciler = new Reconciler(parser, matcher);

        var pair = new Core.Models.FilePair
        {
            FileA = "nonexistent.csv",
            FileB = "other.csv",
            FileAExists = false,
            FileBExists = true
        };

        var result = reconciler.Compare(pair);

        Assert.True(result.IsMissingFile);
    }

    [Fact]
    public void Compare_WithCaseSensitiveMatching_DistinguishesCases()
    {
        using var context = new ReconciliationTestBuilder()
            .WithFileAData(
                "Id,Name",
                "ABC,John",
                "def,Jane")
            .WithFileBData(
                "Id,Name",
                "abc,John",
                "DEF,Jane")
            .WithMatchingFields("Id")
            .WithCaseSensitive(true)
            .Build();

        var parser = new CsvParser(context.Delimiter, true);
        var matcher = new RecordMatcher(context.MatchingConfig);
        var reconciler = new Reconciler(parser, matcher);

        var result = reconciler.Compare(context.FilePair);

        Assert.Equal(0, result.MatchedCount);
        Assert.Equal(2, result.OnlyInACount);
        Assert.Equal(2, result.OnlyInBCount);
    }

    [Fact]
    public void Compare_WithTrimming_IgnoresWhitespace()
    {
        using var context = new ReconciliationTestBuilder()
            .WithFileAData(
                "Id,Name",
                "  1  ,John",
                "2,Jane")
            .WithFileBData(
                "Id,Name",
                "1,John",
                "  2  ,Jane")
            .WithMatchingFields("Id")
            .WithTrim(true)
            .Build();

        var parser = new CsvParser(context.Delimiter, true);
        var matcher = new RecordMatcher(context.MatchingConfig);
        var reconciler = new Reconciler(parser, matcher);

        var result = reconciler.Compare(context.FilePair);

        Assert.Equal(2, result.MatchedCount);
        Assert.Equal(0, result.OnlyInACount);
        Assert.Equal(0, result.OnlyInBCount);
    }
}
