using System.Text;
using System.Text.RegularExpressions;

namespace RachaStats.Application.Matches;

public static class PlayerNameNormalizer
{
    public static string Normalize(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return string.Empty;

        var normalized = name
            .ToLowerInvariant()
            .Normalize(NormalizationForm.FormD);

        normalized = Regex.Replace(normalized, @"\p{Mn}", "");
        normalized = Regex.Replace(normalized, @"[^\p{L}\p{N}\s]", "");
        normalized = Regex.Replace(normalized, @"\s+", " ");

        return normalized.Trim();
    }
}
