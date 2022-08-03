// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.Epub;

public class EpubNavigationItemRef
{
    public EpubNavigationItemRef(EpubNavigationItemType type)
    {
        Type = type;
    }

    public EpubNavigationItemType Type { get; }
    public string Title { get; set; }
    public EpubNavigationItemLink Link { get; set; }
    public EpubTextContentFileRef HtmlContentFileRef { get; set; }
    public List<EpubNavigationItemRef> NestedItems { get; set; }

    public static EpubNavigationItemRef CreateAsHeader()
    {
        return new EpubNavigationItemRef(EpubNavigationItemType.Header);
    }

    public static EpubNavigationItemRef CreateAsLink()
    {
        return new EpubNavigationItemRef(EpubNavigationItemType.Link);
    }

    public override string ToString()
    {
        return $"Type: {Type}, Title: {Title}, NestedItems.Count: {NestedItems.Count}";
    }
}