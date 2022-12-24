// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* HtmlCommandNode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace HtmlAgilityPack;

/// <summary>
/// Represents an HTML comment.
/// </summary>
public class HtmlCommentNode : HtmlNode
{
    #region Fields

    private string _comment;

    #endregion

    #region Constructors

    internal HtmlCommentNode (HtmlDocument ownerDocument, int index)
        : base (HtmlNodeType.Comment, ownerDocument, index)
    {
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or Sets the comment text of the node.
    /// </summary>
    public string Comment
    {
        get
        {
            if (_comment == null)
            {
                return base.InnerHtml;
            }

            return _comment;
        }
        set { _comment = value; }
    }

    /// <summary>
    /// Gets or Sets the HTML between the start and end tags of the object. In the case of a text node, it is equals to OuterHtml.
    /// </summary>
    public override string InnerHtml
    {
        get
        {
            if (_comment == null)
            {
                return base.InnerHtml;
            }

            return _comment;
        }
        set { _comment = value; }
    }

    /// <summary>
    /// Gets or Sets the object and its content in HTML.
    /// </summary>
    public override string OuterHtml
    {
        get
        {
            if (_comment == null)
            {
                return base.OuterHtml;
            }

            return "<!--" + _comment + "-->";
        }
    }

    #endregion
}
