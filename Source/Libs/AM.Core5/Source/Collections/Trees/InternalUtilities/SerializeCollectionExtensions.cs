// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* SerializeCollectionExtension.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace TreeCollections;

internal static class SerializeCollectionExtensions
{
    public static string SerializeToString<T>
        (
            this IEnumerable<T> values
        )
    {
        return values.SerializeToString
            (
                separator: string.Empty,
                wrapperStart: string.Empty,
                wrapperEnd: string.Empty
            );
    }

    public static string SerializeToString<T>
        (
            this IEnumerable<T> values,
            char separator,
            string wrapperStart = "",
            string wrapperEnd = ""
        )
    {
        return values.SerializeToString (separator.ToString(), wrapperStart, wrapperEnd);
    }

    public static string SerializeToString<T>
        (
            this IEnumerable<T> values,
            string separator,
            string wrapperStart = "",
            string wrapperEnd = ""
        )
    {
        var list = values.ToList();

        if (list.Count == 0)
        {
            return string.Empty;
        }

        var result = list.Select (val => $"{wrapperStart}{val}{wrapperEnd}")
            .Aggregate ((a, b) => $"{a}{separator}{b}");

        return result;
    }

    public static string ToCsv<T>
        (
            this IEnumerable<T> values
        )
    {
        return SerializeToString (values, ", ");
    }

    public static string ToSingleQuotedCsv<T>
        (
            this IEnumerable<T> values
        )
    {
        return SerializeToString (values, ", ", "'", "'");
    }

    public static string ToDoubleQuotedCsv<T>
        (
            this IEnumerable<T> values
        )
    {
        return SerializeToString (values, ", ", "\"", "\"");
    }
}
