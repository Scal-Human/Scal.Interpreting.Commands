
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scal.Interpreting.Commands.Extensions;

/// Extensions to the system <see cref="Type"/>.
public static class TypeExtensions
{

    /// Enumerated the type deriving from the given base type and defined in the same assembly.
    /// <param name="baseType">The base type to enumerate derived types from.</param>
    public static IEnumerable<Type> EnumerateDerivedTypes(this Type baseType)
    {
        foreach (var type in baseType.Assembly.GetTypes().Where(type => baseType.IsAssignableFrom(type))) {
            yield return type;
        }
    }
}
