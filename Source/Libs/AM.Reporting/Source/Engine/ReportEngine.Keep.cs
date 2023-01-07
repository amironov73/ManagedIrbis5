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
    #region Fields

    private int _keepPosition;
    private XmlItem _keepOutline;
    private int _keepBookmarks;
    private float _keepCurX;
    private float _keepDeltaY;

    #endregion Fields

    #region Properties

    /// <summary>
    /// Returns true of keeping is enabled
    /// </summary>
    public bool IsKeeping { get; private set; }

    /// <summary>
    /// Returns keeping position
    /// </summary>
    public float KeepCurY { get; private set; }

    #endregion Properties

    #region Private Methods

    private void StartKeep (BandBase band)
    {
        // do not keep the first row on a page, avoid empty first page
        if (IsKeeping || band is { AbsRowNo: 1, FirstRowStartsNewPage: false })
        {
            return;
        }

        IsKeeping = true;

        _keepPosition = PreparedPages.CurPosition;
        _keepOutline = PreparedPages.Outline.CurPosition;
        _keepBookmarks = PreparedPages.Bookmarks.CurPosition;
        KeepCurY = CurY;
        Report.Dictionary.Totals.StartKeep();
        StartKeepReprint();
    }

    private void CutObjects()
    {
        _keepCurX = CurX;
        _keepDeltaY = CurY - KeepCurY;
        PreparedPages.CutObjects (_keepPosition);
        CurY = KeepCurY;
    }

    private void PasteObjects()
    {
        PreparedPages.PasteObjects (CurX - _keepCurX, CurY - KeepCurY);
        PreparedPages.Outline.Shift (_keepOutline, CurY);
        PreparedPages.Bookmarks.Shift (_keepBookmarks, CurY);
        EndKeep();
        CurY += _keepDeltaY;
    }

    #endregion Private Methods

    #region Public Methods

    /// <summary>
    /// Starts the keep mechanism.
    /// </summary>
    /// <remarks>
    /// Use this method along with the <see cref="EndKeep"/> method if you want to keep
    /// several bands together. Call <b>StartKeep</b> method before printing the first band
    /// you want to keep, then call the <b>EndKeep</b> method after printing the last band you want to keep.
    /// </remarks>
    public void StartKeep()
    {
        StartKeep (null);
    }

    /// <summary>
    /// Ends the keep mechanism.
    /// </summary>
    /// <remarks>
    /// Use this method along with the <see cref="StartKeep()"/> method if you want to keep
    /// several bands together. Call <b>StartKeep</b> method before printing the first band
    /// you want to keep, then call the <b>EndKeep</b> method after printing the last band you want to keep.
    /// </remarks>
    public void EndKeep()
    {
        if (IsKeeping)
        {
            Report.Dictionary.Totals.EndKeep();
            EndKeepReprint();
            IsKeeping = false;
        }
    }

    #endregion Public Methods
}
