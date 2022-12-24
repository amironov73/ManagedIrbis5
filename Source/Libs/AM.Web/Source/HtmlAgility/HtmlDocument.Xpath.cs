// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Xml.XPath;

#endregion

#nullable enable

namespace HtmlAgilityPack;

public partial class HtmlDocument
    : IXPathNavigable
{
    /// <summary>
    /// Creates a new XPathNavigator object for navigating this HTML document.
    /// </summary>
    /// <returns>An XPathNavigator object. The XPathNavigator is positioned on the root of the document.</returns>
    public XPathNavigator CreateNavigator()
    {
        return new HtmlNodeNavigator(this, _documentnode);
    }
}
