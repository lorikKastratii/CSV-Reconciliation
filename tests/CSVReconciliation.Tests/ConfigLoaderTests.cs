using CSVReconciliation.Core.Services;

namespace CSVReconciliation.Tests;

public class ConfigLoaderTests
{
    [Fact]
    public void Load_SingleField_ParsesCorrectly()
    {
        var json = @"{ ""matchingFields"": [""InvoiceId""], ""caseSensitive"": false, ""trim"": true }";
        var tempFile = Path.GetTempFileName();
        
        try
        {
            File.WriteAllText(tempFile, json);

            var loader = new ConfigLoader();
            var config = loader.Load(tempFile);

            Assert.Single(config.MatchingFields);
            Assert.Equal("InvoiceId", config.MatchingFields[0]);
            Assert.False(config.CaseSensitive);
            Assert.True(config.Trim);
        }
        finally
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }

    [Fact]
    public void Load_MultipleFields_ParsesCorrectly()
    {
        var json = @"{ ""matchingFields"": [""FirstName"", ""LastName""], ""caseSensitive"": true, ""trim"": false }";
        var tempFile = Path.GetTempFileName();
        
        try
        {
            File.WriteAllText(tempFile, json);

            var loader = new ConfigLoader();
            var config = loader.Load(tempFile);

            Assert.Equal(2, config.MatchingFields.Count);
            Assert.Equal("FirstName", config.MatchingFields[0]);
            Assert.Equal("LastName", config.MatchingFields[1]);
            Assert.True(config.CaseSensitive);
            Assert.False(config.Trim);
        }
        finally
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }
}
