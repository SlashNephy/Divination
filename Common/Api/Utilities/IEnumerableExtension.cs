using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Dalamud.Divination.Common.Api.Utilities;

public static class IEnumerableExtension
{
    public static bool TryGetFirst<T>(this IEnumerable<T> values, out T result) where T : struct {
        using var e = values.GetEnumerator();
        if (e.MoveNext()) {
            result = e.Current;
            return true;
        }
        result = default;
        return false;
    }

    public static bool TryGetFirst<T>(this IEnumerable<T> values, Predicate<T> predicate, out T result) where T : struct {
        using var e = values.GetEnumerator();
        while (e.MoveNext()) {
            if (predicate(e.Current)) {
                result = e.Current;
                return true;
            }
        }
        result = default;
        return false;
    }
}
