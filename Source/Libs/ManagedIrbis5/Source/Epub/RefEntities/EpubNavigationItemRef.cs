// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* EpubNavigationItemRef.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.Epub;

/// <summary>
///
/// </summary>
public class EpubNavigationItemRef
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="type"></param>
    public EpubNavigationItemRef
        (
            EpubNavigationItemType type
        )
    {
        Type = type;
    }

    /// <summary>
    ///
    /// </summary>
    public EpubNavigationItemType Type { get; }

    /// <summary>
    ///
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    ///
    /// </summary>
    public EpubNavigationItemLink? Link { get; set; }

    /// <summary>
    ///
    /// </summary>
    public EpubTextContentFileRef? HtmlContentFileRef { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<EpubNavigationItemRef>? NestedItems { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static EpubNavigationItemRef CreateAsHeader()
    {
        return new EpubNavigationItemRef (EpubNavigationItemType.Header);
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static EpubNavigationItemRef CreateAsLink()
    {
        return new EpubNavigationItemRef (EpubNavigationItemType.Link);
    }

    /// <inheritdoc cref="System.Object.ToString"/>
    public override string ToString()
    {
        return $"Type: {Type}, Title: {Title}, NestedItems.Count: {NestedItems?.Count}";
    }
}
