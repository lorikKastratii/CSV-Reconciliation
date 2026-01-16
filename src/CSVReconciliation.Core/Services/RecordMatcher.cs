using CSVReconciliation.Core.Models;

namespace CSVReconciliation.Core.Services;

public class RecordMatcher
{
    private MatchingConfig _config;

    public RecordMatcher(MatchingConfig config)
    {
        _config = config;
    }

    public string GetKey(CsvRecord record)
    {
        var parts = new List<string>();

        foreach (var field in _config.MatchingFields)
        {
            var value = record.GetValue(field) ?? "";

            if (_config.Trim)
                value = value.Trim();

            if (!_config.CaseSensitive)
                value = value.ToLower();

            parts.Add(value);
        }

        return string.Join("|", parts);
    }
}
