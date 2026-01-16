using CSVReconciliation.Core.Services;
using CSVReconciliation.Tests.Builders;

namespace CSVReconciliation.Tests;

public class CsvParserTests
{
    [Fact]
    public void Parse_WithHeader_ReadsFieldNames()
    {
        using var context = new ReconciliationTestBuilder()
            .WithFileAData(
                "Id,Name",
                "1,John",
                "2,Jane")
            .Build();

        var parser = new CsvParser(context.Delimiter, true);
        var errors = new List<string>();
        var records = parser.Parse(context.FileA, errors);

        Assert.Equal(2, records.Count);
        Assert.Equal("1", records[0].Fields["Id"]);
        Assert.Equal("John", records[0].Fields["Name"]);
        Assert.Equal("2", records[1].Fields["Id"]);
        Assert.Equal("Jane", records[1].Fields["Name"]);
    }

    [Fact]
    public void Parse_WithoutHeader_UsesColumnNumbers()
    {
        using var context = new ReconciliationTestBuilder()
            .WithFileAData(
                "1,John",
                "2,Jane")
            .Build();

        var parser = new CsvParser(context.Delimiter, false);
        var errors = new List<string>();
        var records = parser.Parse(context.FileA, errors);

        Assert.Equal(2, records.Count);
        Assert.Equal("1", records[0].Fields["Column1"]);
        Assert.Equal("John", records[0].Fields["Column2"]);
    }

    [Fact]
    public void Parse_EmptyLines_SkipsEmpty()
    {
        using var context = new ReconciliationTestBuilder()
            .WithFileAData(
                "Id,Name",
                "1,John",
                "",
                "2,Jane")
            .Build();

        var parser = new CsvParser(context.Delimiter, true);
        var errors = new List<string>();
        var records = parser.Parse(context.FileA, errors);

        Assert.Equal(2, records.Count);
    }

    [Fact]
    public void GetHeaders_ReturnsFirstRow()
    {
        using var context = new ReconciliationTestBuilder()
            .WithFileAData(
                "Id,Name,Age",
                "1,John,30")
            .Build();

        var parser = new CsvParser(context.Delimiter, true);
        var headers = parser.GetHeaders(context.FileA);

        Assert.Equal(3, headers.Length);
        Assert.Equal("Id", headers[0]);
        Assert.Equal("Name", headers[1]);
        Assert.Equal("Age", headers[2]);
    }

    [Fact]
    public void Parse_WithTabDelimiter_ParsesCorrectly()
    {
        using var context = new ReconciliationTestBuilder()
            .WithFileAData(
                "Id\tName",
                "1\tJohn",
                "2\tJane")
            .WithDelimiter('\t')
            .Build();

        var parser = new CsvParser(context.Delimiter, true);
        var errors = new List<string>();
        var records = parser.Parse(context.FileA, errors);

        Assert.Equal(2, records.Count);
        Assert.Equal("1", records[0].Fields["Id"]);
        Assert.Equal("John", records[0].Fields["Name"]);
    }
}
