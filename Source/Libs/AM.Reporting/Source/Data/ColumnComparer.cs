// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ColumnComparer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;

#endregion

#nullable enable

namespace AM.Reporting.Data;

/// <summary>
/// Represents the comparer class that used for sorting the collection of columns.
/// </summary>
public class ColumnComparer
    : IComparer
{
    /// <inheritdoc cref="IComparer.Compare"/>
    public int Compare
        (
            object? x,
            object? y
        )
    {
        var xValue = x?.GetType().GetProperty ("Name")?.GetValue (x, null);
        var yValue = y?.GetType().GetProperty ("Name")?.GetValue (y, null);
        return xValue is not IComparable comp
            ? -1
            : comp.CompareTo (yValue);
    }
}
