using System.Text.Json;
using CSVReconciliation.Core.Models;

namespace CSVReconciliation.Core.Services;

public class ConfigLoader
{
    public MatchingConfig Load(string path)
    {
        var json = File.ReadAllText(path);
        var config = JsonSerializer.Deserialize<MatchingConfig>(json);
        return config;
    }
}
