// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* EpubNavigationItem.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Epub;

/// <summary>
///
/// </summary>
public sealed class EpubNavigationItem
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public EpubNavigationItemType Type { get; }

    /// <summary>
    ///
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    ///
    /// </summary>
    public EpubNavigationItemLink Link { get; set; }

    /// <summary>
    ///
    /// </summary>
    public EpubTextContentFile HtmlContentFile { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<EpubNavigationItem> NestedItems { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public EpubNavigationItem
        (
            EpubNavigationItemType type
        )
    {
        Sure.Defined (type);

        Type = type;
    }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    public static EpubNavigationItem CreateAsHeader()
    {
        return new EpubNavigationItem (EpubNavigationItemType.Header);
    }

    /// <summary>
    ///
    /// </summary>
    public static EpubNavigationItem CreateAsLink()
    {
        return new EpubNavigationItem (EpubNavigationItemType.Link);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"Type: {Type}, Title: {Title}, NestedItems.Count: {NestedItems.Count}";
    }

    #endregion
}
