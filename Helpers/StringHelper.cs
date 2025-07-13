using System.Text;
using System.Text.RegularExpressions;

namespace RouterPlus.Helpers;

public class StringHelper
{
    public static bool AreEquivalent(string source, string target)
    {
        if (source == null)
        {
            return target == null;
        }

        if (target == null)
        {
            return false;
        }

        var normForm1 = Normalize(source);
        var normForm2 = Normalize(target);
        return string.Equals(normForm1, normForm2);
    }

    private static string Normalize(string value)
    {
        value = value.Normalize(NormalizationForm.FormC);
        value = value.Replace("\r\n", "\n").Replace("\r", "\n");
        value = Regex.Replace(value, @"\s", string.Empty);

        return value.ToLowerInvariant();
    }
}