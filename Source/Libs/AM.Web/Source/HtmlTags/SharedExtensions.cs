// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

#nullable enable

namespace AM.HtmlTags;

internal static class SharedExtensions
{
    public static void Each<T> (this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (T item in enumerable)
        {
            action (item);
        }
    }

    public static string[] ToDelimitedArray (this string content, params char[] delimiter) =>
        content.Split (delimiter).Select (x => x.Trim()).ToArray();

    public static string Join (this IEnumerable<string> strings, string separator) =>
        string.Join (separator, strings);
}
