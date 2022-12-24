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

#nullable enable

namespace HtmlAgilityPack;

/// <summary>
/// Represents an HTML text node.
/// </summary>
public class HtmlTextNode : HtmlNode
{
    #region Fields

    private string _text;

    #endregion

    #region Constructors

    internal HtmlTextNode (HtmlDocument ownerDocument, int index)
        :
        base (HtmlNodeType.Text, ownerDocument, index)
    {
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or Sets the HTML between the start and end tags of the object. In the case of a text node, it is equals to OuterHtml.
    /// </summary>
    public override string InnerHtml
    {
        get { return OuterHtml; }
        set { _text = value; }
    }

    /// <summary>
    /// Gets or Sets the object and its content in HTML.
    /// </summary>
    public override string OuterHtml
    {
        get
        {
            if (_text == null)
            {
                return base.OuterHtml;
            }

            return _text;
        }
    }

    /// <summary>
    /// Gets or Sets the text of the node.
    /// </summary>
    public string Text
    {
        get
        {
            if (_text == null)
            {
                return base.OuterHtml;
            }

            return _text;
        }
        set
        {
            _text = value;
            SetChanged();
        }
    }

    #endregion
}
