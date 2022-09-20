// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Xml.XPath;

#endregion

#nullable enable

using AM;

namespace HtmlAgilityPack;

public partial class HtmlNode
    : IXPathNavigable
{
    /// <summary>
    /// Creates a new XPathNavigator object for navigating this HTML node.
    /// </summary>
    /// <returns>An XPathNavigator object. The XPathNavigator is positioned on the node from which the method was called. It is not positioned on the root of the document.</returns>
    public XPathNavigator CreateNavigator()
    {
        return new HtmlNodeNavigator (OwnerDocument, this);
    }

    /// <summary>
    /// Creates an XPathNavigator using the root of this document.
    /// </summary>
    /// <returns></returns>
    public XPathNavigator CreateRootNavigator()
    {
        return new HtmlNodeNavigator (OwnerDocument, OwnerDocument.DocumentNode);
    }

    /// <summary>
    /// Selects a list of nodes matching the <see cref="XPath"/> expression.
    /// </summary>
    /// <param name="xpath">The XPath expression.</param>
    /// <returns>An <see cref="HtmlNodeCollection"/> containing a collection of nodes matching the <see cref="XPath"/> query, or <c>null</c> if no node matched the XPath expression.</returns>
    public HtmlNodeCollection? SelectNodes (string xpath)
    {
        var list = new HtmlNodeCollection (null);

        var nav = new HtmlNodeNavigator (OwnerDocument, this);
        var it = nav.Select (xpath);
        while (it.MoveNext())
        {
            var n = (HtmlNodeNavigator)it.Current;
            list.Add (n.CurrentNode, false);
        }

        if (list.Count == 0 && !OwnerDocument.OptionEmptyCollection)
        {
            return null;
        }

        return list;
    }

    /// <summary>
    /// Selects a list of nodes matching the <see cref="XPath"/> expression.
    /// </summary>
    /// <param name="xpath">The XPath expression.</param>
    /// <returns>An <see cref="HtmlNodeCollection"/> containing a collection of nodes matching the <see cref="XPath"/> query, or <c>null</c> if no node matched the XPath expression.</returns>
    public HtmlNodeCollection? SelectNodes (XPathExpression xpath)
    {
        var list = new HtmlNodeCollection (null);

        var nav = new HtmlNodeNavigator (OwnerDocument, this);
        var it = nav.Select (xpath);
        while (it.MoveNext())
        {
            var n = (HtmlNodeNavigator)it.Current;
            list.Add (n.CurrentNode, false);
        }

        if (list.Count == 0 && !OwnerDocument.OptionEmptyCollection)
        {
            return null;
        }

        return list;
    }

    /// <summary>
    /// Selects the first XmlNode that matches the <see cref="XPath"/> expression.
    /// </summary>
    /// <param name="xpath">The XPath expression. May not be null.</param>
    /// <returns>The first <see cref="HtmlNode"/> that matches the XPath query or a null reference if no matching node was found.</returns>
    public HtmlNode? SelectSingleNode
        (
            string xpath
        )
    {
        Sure.NotNull (xpath);

        var nav = new HtmlNodeNavigator (OwnerDocument, this);
        var it = nav.Select (xpath);
        if (!it.MoveNext())
        {
            return null;
        }

        var node = (HtmlNodeNavigator)it.Current;
        return node.CurrentNode;
    }

    /// <summary>
    /// Selects the first XmlNode that matches the <see cref="XPath"/> expression.
    /// </summary>
    /// <param name="xpath">The XPath expression.</param>
    /// <returns>An <see cref="HtmlNodeCollection"/> containing a collection of nodes matching the <see cref="XPath"/> query, or <c>null</c> if no node matched the XPath expression.</returns>
    public HtmlNode SelectSingleNode
        (
            XPathExpression xpath
        )
    {
        Sure.NotNull (xpath);

        var nav = new HtmlNodeNavigator (OwnerDocument, this);
        var it = nav.Select (xpath);
        if (!it.MoveNext())
        {
            return null;
        }

        var node = (HtmlNodeNavigator)it.Current;
        return node.CurrentNode;
    }
}
