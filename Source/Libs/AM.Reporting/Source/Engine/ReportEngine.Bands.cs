// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ReportEngine.Bands.cs--
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Reporting.Engine;

public partial class ReportEngine
{
    #region Fields

    private BandBase? _outputBand;

    #endregion Fields

    #region Private Methods

    private void PrepareBand
        (
            BandBase band,
            bool getData
        )
    {
        if (band.Visible)
        {
            if (getData)
            {
                band.GetData();
            }

            TranslateObjects (band);
            RenderInnerSubreports (band);
            band.CalcHeight();
        }
    }

    private float CalcHeight
        (
            BandBase band
        )
    {
        // band is already prepared, its Height is ready to use
        if (band.IsRunning)
        {
            return band.Height;
        }

        band.SaveState();
        try
        {
            PrepareBand (band, true);
            return band.Height;
        }
        finally
        {
            band.RestoreState();
        }
    }

    private BandBase CloneBand
        (
            BandBase band
        )
    {
        // clone a band and all its objects
        var cloneBand = (BandBase) Activator.CreateInstance (band.GetType()).ThrowIfNull();
        cloneBand.Assign (band);
        cloneBand.SetReport (Report);
        cloneBand.SetRunning (true);

        foreach (ReportComponentBase obj in band.Objects)
        {
            var cloneObj = (ReportComponentBase) Activator.CreateInstance (obj.GetType()).ThrowIfNull();
            cloneObj.AssignAll (obj);
            cloneBand.Objects.Add (cloneObj);
        }

        return cloneBand;
    }

    private void AddToOutputBand
        (
            BandBase band,
            bool getData
        )
    {
        band.SaveState();

        try
        {
            PrepareBand (band, getData);

            if (band.Visible)
            {
                _outputBand.SetRunning (true);

                var cloneBand = CloneBand (band);
                cloneBand.Left = CurX;
                cloneBand.Top = CurY;
                cloneBand.Parent = _outputBand;

                CurY += cloneBand.Height;
            }
        }
        finally
        {
            band.RestoreState();
        }
    }

    private void ShowBandToPreparedPages
        (
            BandBase band,
            bool getData
        )
    {
        // handle "StartNewPage". Skip if it's the first row, avoid empty first page.
        if (band.StartNewPage && band.Parent is not (PageHeaderBand or PageFooterBand) &&
            band.FlagUseStartNewPage && (band.RowNo != 1 || band.FirstRowStartsNewPage) &&
            !band.Repeated)
        {
            EndColumn();
        }

        band.SaveState();
        try
        {
            PrepareBand (band, getData);

            if (band.Visible)
            {
                if (BandHasHardPageBreaks (band))
                {
                    foreach (var b in SplitHardPageBreaks (band))
                    {
                        if (b.StartNewPage)
                        {
                            EndColumn();
                        }

                        AddToPreparedPages (b);
                    }
                }
                else
                {
                    AddToPreparedPages (band);
                }
            }
        }
        finally
        {
            band.RestoreState();
        }
    }

    private void ShowBand
        (
            BandBase band,
            BandBase outputBand,
            float offsetX,
            float offsetY
        )
    {
        var saveCurX = CurX;
        var saveCurY = CurY;
        var saveOutputBand = this._outputBand;
        CurX = offsetX;
        CurY = offsetY;

        try
        {
            this._outputBand = outputBand;
            ShowBand (band);
        }
        finally
        {
            this._outputBand = saveOutputBand;
            CurX = saveCurX;
            CurY = saveCurY;
        }
    }

    /// <summary>
    /// Shows band at the current position.
    /// </summary>
    /// <param name="band">Band to show.</param>
    /// <remarks>
    /// After the band is shown, the current position is advanced by the band's height.
    /// </remarks>
    public void ShowBand (BandBase? band)
    {
        if (band is not null)
        {
            for (var i = 0; i < band.RepeatBandNTimes; i++)
            {
                ShowBand (band, true);
            }
        }
    }

    private void ShowBand (BandBase? band, bool getData)
    {
        if (band is null)
        {
            return;
        }

        var saveCurBand = _curBand;
        _curBand = band;

        try
        {
            // do we need to keep child?
            var child = band.Child;
            var showChild = child != null && !(band is DataBand && child.CompleteToNRows > 0) &&
                            !child.FillUnusedSpace &&
                            !(band is DataBand && child.PrintIfDatabandEmpty);
            if (showChild && band.KeepChild)
            {
                StartKeep (band);
            }

            if (_outputBand != null)
            {
                AddToOutputBand (band, getData);
            }
            else
            {
                ShowBandToPreparedPages (band, getData);
            }

            ProcessTotals (band);
            if (band.Visible)
            {
                RenderOuterSubreports (band);
            }

            // show child band. Skip if child is used to fill empty space: it was processed already
            if (showChild)
            {
                ShowBand (child);
                if (band.KeepChild)
                {
                    EndKeep();
                }
            }
        }
        finally
        {
            _curBand = saveCurBand;
        }
    }

    private void ProcessTotals (BandBase band)
    {
        Report.Dictionary.Totals.ProcessBand (band);
    }

    #endregion Private Methods

    #region Internal Methods

    internal bool CanPrint (ReportComponentBase obj)
    {
        // Apply visible expression if needed.
        if (!string.IsNullOrEmpty (obj.VisibleExpression))
        {
            object expression = null;

            // Calculate expressions with TotalPages only on FinalPass.
            if (!obj.VisibleExpression.Contains ("TotalPages") || (Report.DoublePass && FinalPass))
            {
                expression = Report.Calc (Code.CodeUtils.FixExpressionWithBrackets (obj.VisibleExpression));
            }

            if (expression is bool b)
            {
                if (!obj.VisibleExpression.Contains ("TotalPages"))
                {
                    obj.Visible = b;
                }
                else if (FirstPass)
                {
                    obj.Visible = true;
                }
                else
                {
                    obj.Visible = b;
                }
            }
        }

        // Apply exportable expression if needed.
        if (!string.IsNullOrEmpty (obj.ExportableExpression))
        {
            object? expression = null;
            expression = Report.Calc (Code.CodeUtils.FixExpressionWithBrackets (obj.ExportableExpression));
            if (expression is bool b)
            {
                obj.Exportable = b;
            }
        }

        // Apply printable expression if needed.
        if (!string.IsNullOrEmpty (obj.PrintableExpression))
        {
            object? expression = null;
            expression = Report.Calc (Code.CodeUtils.FixExpressionWithBrackets (obj.PrintableExpression));
            if (expression is bool b)
            {
                obj.Printable = b;
            }
        }

        if (!obj.Visible || !obj.FlagPreviewVisible)
        {
            return false;
        }

        var isFirstPage = CurPage == _firstReportPage;
        var isLastPage = CurPage == TotalPages - 1;
        var isRepeated = obj.Band is { Repeated: true };
        var canPrint = false;

        if ((obj.PrintOn & PrintOn.OddPages) > 0 && CurPage % 2 == 1)
        {
            canPrint = true;
        }

        if ((obj.PrintOn & PrintOn.EvenPages) > 0 && CurPage % 2 == 0)
        {
            canPrint = true;
        }

        if (isLastPage)
        {
            if ((obj.PrintOn & PrintOn.LastPage) == 0)
            {
                canPrint = false;
            }

            if (obj.PrintOn == PrintOn.LastPage || obj.PrintOn == (PrintOn.LastPage | PrintOn.SinglePage) ||
                obj.PrintOn == (PrintOn.FirstPage | PrintOn.LastPage))
            {
                canPrint = true;
            }
        }

        if (isFirstPage)
        {
            if ((obj.PrintOn & PrintOn.FirstPage) == 0)
            {
                canPrint = false;
            }

            if (obj.PrintOn == PrintOn.FirstPage || obj.PrintOn == (PrintOn.FirstPage | PrintOn.SinglePage) ||
                obj.PrintOn == (PrintOn.FirstPage | PrintOn.LastPage))
            {
                canPrint = true;
            }
        }

        if (isFirstPage && isLastPage)
        {
            canPrint = (obj.PrintOn & PrintOn.SinglePage) > 0;
        }

        if (isRepeated)
        {
            canPrint = (obj.PrintOn & PrintOn.RepeatedBand) > 0;
        }

        return canPrint;
    }

    internal void AddToPreparedPages (BandBase band)
    {
        var isReportSummary = band is ReportSummaryBand;

        // check if band is service band (e.g. page header/footer/overlay).
        var mainBand = band;

        // for child bands, check its parent band.
        if (band is ChildBand childBand)
        {
            mainBand = childBand.GetTopParentBand;
        }

        var isPageBand = mainBand is PageHeaderBand or PageFooterBand or OverlayBand;
        var isColumnBand = mainBand is ColumnHeaderBand or ColumnFooterBand;

        // check if we have enough space for a band.
        var checkFreeSpace = !isPageBand && !isColumnBand && band.FlagCheckFreeSpace;
        if (checkFreeSpace && FreeSpace < band.Height)
        {
            // we don't have enough space. What should we do?
            // - if band can break, break it
            // - if band cannot break, check the band height:
            //   - it's the first row of a band and is bigger than page: break it immediately.
            //   - in other case, add a new page/column and tell the band that it must break next time.
            if (band.CanBreak || band.FlagMustBreak ||
                (band.AbsRowNo == 1 && band.Height > PageHeight - PageFooterHeight))
            {
                // since we don't show the column footer band in the EndLastPage, do it here.
                if (isReportSummary)
                {
                    ShowReprintFooters();
                    ShowBand (_page.ColumnFooter);
                }

                BreakBand (band);
                return;
            }
            else
            {
                EndColumn();
                band.FlagMustBreak = true;
                AddToPreparedPages (band);
                band.FlagMustBreak = false;
                return;
            }
        }
        else
        {
            // since we don't show the column footer band in the EndLastPage, do it here.
            if (isReportSummary)
            {
                if ((band as ReportSummaryBand).KeepWithData)
                {
                    EndKeep();
                }

                ShowReprintFooters (false);
                ShowBand (_page.ColumnFooter);
            }
        }

        // check if we have a child band with FillUnusedSpace flag
        if (band.Child is { FillUnusedSpace: true })
        {
            // if we reprint a data/group footer, do not include the band height into calculation:
            // it is already counted in FreeSpace
            var bandHeight = band.Height;
            if (band.Repeated)
            {
                bandHeight = 0;
            }

            while (FreeSpace - bandHeight - band.Child.Height >= 0)
            {
                var saveCurY = CurY;
                ShowBand (band.Child);

                // nothing was printed, break to avoid an endless loop
                if (CurY == saveCurY)
                {
                    break;
                }
            }
        }

        // adjust the band location
        if (band is PageFooterBand && !UnlimitedHeight)
        {
            CurY = PageHeight - GetBandHeightWithChildren (band);
        }

        if (!isPageBand)
        {
            band.Left += _originX + CurX;
        }

        if (band.PrintOnBottom)
        {
            CurY = PageHeight - PageFooterHeight - ColumnFooterHeight;

            // if PrintOnBottom is applied to a band like DataFooter, print it with all its child bands
            // if PrintOnBottom is applied to a child band, print this band only.
            if (band is ChildBand)
            {
                CurY -= band.Height;
            }
            else
            {
                CurY -= GetBandHeightWithChildren (band);
            }
        }

        band.Top = CurY;

        // shift the band and decrease its width when printing hierarchy
        var saveLeft = band.Left;
        var saveWidth = band.Width;
        if (!isPageBand && !isColumnBand)
        {
            band.Left += _hierarchyIndent;
            band.Width -= _hierarchyIndent;
        }

        // add outline
        AddBandOutline (band);

        // add bookmarks
        band.AddBookmarks();

        // put the band to prepared pages. Do not put page bands twice
        // (this may happen when we render a subreport, or append a report to another one).
        var bandAdded = true;
        var bandAlreadyExists = false;
        if (isPageBand)
        {
            if (band is ChildBand)
            {
                bandAlreadyExists = PreparedPages.ContainsBand (band.Name);
            }
            else
            {
                bandAlreadyExists = PreparedPages.ContainsBand (band.GetType());
            }
        }

        if (!bandAlreadyExists)
        {
            bandAdded = PreparedPages.AddBand (band);
        }

        // shift CurY
        if (bandAdded && mainBand is not OverlayBand)
        {
            CurY += band.Height;
        }

        // set left&width back
        band.Left = saveLeft;
        band.Width = saveWidth;
    }

    #endregion Internal Methods
}
