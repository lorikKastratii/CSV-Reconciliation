using System.Text.Json.Serialization;

namespace CSVReconciliation.Core.Models;

public class MatchingConfig
{
    [JsonPropertyName("matchingFields")]
    public List<string> MatchingFields { get; set; } = new();

    [JsonPropertyName("caseSensitive")]
    public bool CaseSensitive { get; set; } = false;

    [JsonPropertyName("trim")]
    public bool Trim { get; set; } = true;
}
