using CSVReconciliation.Core.Models;
using CSVReconciliation.Core.Services;
using CSVReconciliation.Tests.Builders;

namespace CSVReconciliation.Tests;

public class RecordMatcherTests
{
    [Fact]
    public void GetKey_WithTrim_TrimsWhitespace()
    {
        using var context = new ReconciliationTestBuilder()
            .WithMatchingFields("Name")
            .WithTrim(true)
            .WithCaseSensitive(false)
            .Build();

        var matcher = new RecordMatcher(context.MatchingConfig);
        var record = new CsvRecord();
        record.Fields["Name"] = "  John  ";

        var key = matcher.GetKey(record);

        Assert.Equal("john", key);
    }

    [Fact]
    public void GetKey_CaseInsensitive_ConvertsToLower()
    {
        using var context = new ReconciliationTestBuilder()
            .WithMatchingFields("Name")
            .WithTrim(false)
            .WithCaseSensitive(false)
            .Build();

        var matcher = new RecordMatcher(context.MatchingConfig);
        var record = new CsvRecord();
        record.Fields["Name"] = "JOHN";

        var key = matcher.GetKey(record);

        Assert.Equal("john", key);
    }

    [Fact]
    public void GetKey_CaseSensitive_KeepsCase()
    {
        using var context = new ReconciliationTestBuilder()
            .WithMatchingFields("Name")
            .WithTrim(false)
            .WithCaseSensitive(true)
            .Build();

        var matcher = new RecordMatcher(context.MatchingConfig);
        var record = new CsvRecord();
        record.Fields["Name"] = "JOHN";

        var key = matcher.GetKey(record);

        Assert.Equal("JOHN", key);
    }

    [Fact]
    public void GetKey_MultipleFields_CombinesWithPipe()
    {
        using var context = new ReconciliationTestBuilder()
            .WithMatchingFields("FirstName", "LastName")
            .WithTrim(true)
            .WithCaseSensitive(false)
            .Build();

        var matcher = new RecordMatcher(context.MatchingConfig);
        var record = new CsvRecord();
        record.Fields["FirstName"] = "John";
        record.Fields["LastName"] = "Doe";

        var key = matcher.GetKey(record);

        Assert.Equal("john|doe", key);
    }
}
