// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* TableTag.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.HtmlTags;

/// <summary>
///
/// </summary>
[PublicAPI]
public class TableTag
    : HtmlTag
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public HtmlTag THead { get; }

    /// <summary>
    ///
    /// </summary>
    public HtmlTag TBody { get; }

    /// <summary>
    ///
    /// </summary>
    public HtmlTag TFoot { get; }

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    public TableTag()
        : base ("table")
    {
        THead = new HtmlTag ("thead", this);
        TFoot = new HtmlTag ("tfoot", this).Render (false);
        TBody = new HtmlTag ("tbody", this);
    }

    #endregion

    /// <summary>
    ///
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public TableTag CaptionText (string text)
    {
        var caption = ExistingCaption();
        if (caption == null)
        {
            caption = new HtmlTag ("caption");
            Children.Insert (0, caption);
        }

        caption.Text (text);

        return this;
    }

    #region Private members

    private HtmlTag? ExistingCaption()
    {
        return Children.FirstOrDefault (x => x.TagName() == "caption");
    }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public string CaptionText()
    {
        var caption = ExistingCaption();
        return caption == null ? string.Empty : caption.Text();
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public TableRowTag AddHeaderRow()
    {
        return THead.Add<TableRowTag>();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="configure"></param>
    /// <returns></returns>
    public TableTag AddHeaderRow
        (
            Action<TableRowTag> configure
        )
    {
        configure (AddHeaderRow());

        return this;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public TableRowTag AddBodyRow()
    {
        return TBody.Add<TableRowTag>();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="configure"></param>
    /// <returns></returns>
    public TableTag AddBodyRow
        (
            Action<TableRowTag> configure
        )
    {
        configure (AddBodyRow());
        return this;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="configure"></param>
    /// <returns></returns>
    public TableTag AddFooterRow
        (
            Action<TableRowTag> configure
        )
    {
        TFoot.Render (true).NotUsed();
        configure (TFoot.Add<TableRowTag>());
        return this;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="caption"></param>
    /// <returns></returns>
    public TableTag Caption
        (
            string caption
        )
    {
        var captionTag = ExistingCaption();
        if (captionTag == null)
        {
            captionTag = new HtmlTag ("caption");
            Children.Insert (0, captionTag);
        }

        captionTag.Text (caption);

        return this;
    }

    #endregion
}

/// <summary>
///
/// </summary>
public class TableRowTag
    : HtmlTag
{
    #region Construction

    /// <summary>
    ///
    /// </summary>
    public TableRowTag()
        : base ("tr")
    {
        // пустое тело конструктора
    }

    #endregion

    #region Public methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public HtmlTag Header (string text) => new HtmlTag ("th", this).Text (text);

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public HtmlTag Header() => new ("th", this);

    /// <summary>
    ///
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public HtmlTag Cell (string text) => new HtmlTag ("td", this).Text (text);

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public HtmlTag Cell() => new ("td", this);

    #endregion
}
