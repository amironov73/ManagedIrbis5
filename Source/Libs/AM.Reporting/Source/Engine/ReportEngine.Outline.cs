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

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Engine;

public partial class ReportEngine
{
    #region Properties

    /// <summary>
    /// Gets xml containing outline nodes.
    /// </summary>
    public XmlItem OutlineXml => PreparedPages.Outline.Xml;

    #endregion Properties

    #region Private Methods

    private void AddOutline (string name, int pageNo, float curY)
    {
        PreparedPages.Outline.Add (name, pageNo, curY);
    }

    private void AddBandOutline (BandBase band)
    {
        if (band.Visible && !string.IsNullOrEmpty (band.OutlineExpression) && !band.Repeated)
        {
            AddOutline (Converter.ToString (Report.Calc (band.OutlineExpression)), CurPage, CurY);
            if (band is not DataBand && band is not GroupHeaderBand)
            {
                OutlineUp();
            }
        }
    }

    private void AddPageOutline()
    {
        if (!string.IsNullOrEmpty (_page.OutlineExpression))
        {
            AddOutline (Converter.ToString (Report.Calc (_page.OutlineExpression)), CurPage, 0);
        }
    }

    private void OutlineUp (BandBase band)
    {
        if (band is DataBand or GroupHeaderBand)
        {
            if (!string.IsNullOrEmpty (band.OutlineExpression))
            {
                OutlineUp();
            }
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Creates a new outline element with specified text.
    /// </summary>
    /// <param name="text">Text of element.</param>
    /// <remarks>
    /// After you call this method, the element will be added to the current position in the outline.
    /// The next call to <b>AddOutline</b> will add new element as a child of this element.
    /// To shift the position, use the <see cref="OutlineRoot"/> or
    /// <see cref="OutlineUp()">OutlineUp</see> methods.
    /// </remarks>
    public void AddOutline (string text)
    {
        AddOutline (text, CurPage, CurY);
    }

    /// <summary>
    /// Sets the current outline position to root.
    /// </summary>
    public void OutlineRoot()
    {
        PreparedPages.Outline.LevelRoot();
    }

    /// <summary>
    /// Shifts the current outline position one level up.
    /// </summary>
    public void OutlineUp()
    {
        PreparedPages.Outline.LevelUp();
    }

    /// <summary>
    /// Creates a new bookmark with specified name at current position.
    /// </summary>
    /// <param name="name"></param>
    public void AddBookmark (string name)
    {
        if (!string.IsNullOrEmpty (name))
        {
            PreparedPages.Bookmarks.Add (name, CurPage, CurY);
        }
    }

    /// <summary>
    /// Gets a page number for the specified bookmark name.
    /// </summary>
    /// <param name="name">Name of bookmark.</param>
    /// <returns>Page number if bookmark with such name found; 0 otherwise.</returns>
    /// <remarks>
    /// Use this method to print the table of contents in your report. Normally it can be done
    /// using bookmarks.
    /// <note type="caution">
    /// You must set your report to double pass to use this method.
    /// </note>
    /// </remarks>
    public int GetBookmarkPage (string name)
    {
        return PreparedPages.Bookmarks.GetPageNo (name);
    }

    #endregion
}
