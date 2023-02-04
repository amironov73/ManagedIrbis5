// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* VisualExtensions.cs -- полезные расширения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using Avalonia;
using Avalonia.Layout;
using Avalonia.VisualTree;

#endregion

#nullable enable

namespace AM.Avalonia.Screenshoting;

/// <summary>
/// Некоторые полезные расширения.
/// </summary>
public static class VisualExtensions
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="layoutables"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public static IEnumerable<Layoutable> Layout
        (
            this IEnumerable<Layoutable> layoutables,
            Size size
        )
    {
        var enumerable = layoutables as Layoutable[] ?? layoutables.ToArray();
        foreach (var layoutable in enumerable)
        {
            layoutable.Measure (size);
            layoutable.Arrange (new Rect (layoutable.DesiredSize));
        }

        return enumerable;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="root"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<T> FindAllVisuals<T>
        (
            this Visual root
        )
    {
        var result = new List<T>();
        FindAllVisuals (root, result);

        return result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="root"></param>
    /// <param name="result"></param>
    /// <typeparam name="T"></typeparam>
    private static void FindAllVisuals<T>
        (
            Visual root,
            ICollection<T> result
        )
    {
        if (root is Layoutable l)
        {
            l.ApplyTemplate();
        }

        if (root is T t)
        {
            result.Add (t);
        }

        // foreach (var child in root.VisualChildren)
        // {
        //     FindAllVisuals (child, result);
        // }
    }
}
