using System;
using System.Collections.Generic;
using System.Linq;

namespace Scal.Interpreting.Commands.Extensions;

/// Extensions to the <see cref="string"/> type.
public static class StringExtensions
{

    /// Get the minimal abbreviation needed to identify a name amongst its siblings.
    /// <param name="name">The name to get the abbreviation for.</param>
    /// <param name="siblings">The siblings of the name to check for abbreviation collisions.</param>
    /// <param name="comparison">The string comparison to used.</param>
    public static string GetMinimalAbbreviation(this string name, IEnumerable<string> siblings,
        StringComparison comparison = StringComparison.OrdinalIgnoreCase
    ) {
        var others = siblings.Where(sibling => ! name.Equals(sibling, comparison)).Distinct().ToArray();
        for (int length = 1; length <= name.Length; length++) {
            var prefix = name[..length];
            int matches = others.Count(other => other.StartsWith(prefix, comparison));
            if (matches == 0) {
                return prefix;
            }
        }
        return name;
    }

    /// Get the minimal abbreviation needed to identify a name amongst its siblings.
    /// <param name="name">.</param>
    public static IEnumerable<string> SplitCamelCase(this string name)
    {
        var start = 0;
        var index = 1;
        while (index < name.Length) {
            if (char.IsUpper(name[index])) {
                yield return name.Substring(start, index - start);
                start = index;
            }
            index ++;
        }
        if (index > start) {
            yield return name.Substring(start, index - start);
        }
    }    
}
