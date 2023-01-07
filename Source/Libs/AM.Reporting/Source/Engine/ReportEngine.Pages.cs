// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Reporting.Utils;

using System;

#endregion

#nullable enable

namespace AM.Reporting.Engine
{
    public partial class ReportEngine
    {
        #region Fields

        private ReportPage _page;
        private float _columnStartY;
        private string? _pageNameForRecalc;

        #endregion Fields

        #region Private Methods

        private DataBand? FindDeepmostDataBand (ReportPage page)
        {
            DataBand? result = null;
            foreach (Base c in page.AllObjects)
            {
                if (c is DataBand band)
                {
                    result = band;
                }
            }

            return result;
        }

        private void RunReportPage (ReportPage page)
        {
            _page = page;
            InitReprint();
            _pageNameForRecalc = null;
            _page.OnStartPage (EventArgs.Empty);
            var previousPage = StartFirstPage();
            OnStateChanged (_page, EngineState.ReportPageStarted);
            OnStateChanged (_page, EngineState.PageStarted);

            var keepSummaryBand = FindDeepmostDataBand (page);
            if (keepSummaryBand != null)
            {
                keepSummaryBand.KeepSummary = true;
            }

            if (_page.IsManualBuild)
            {
                _page.OnManualBuild (EventArgs.Empty);
            }
            else
            {
                RunBands (page.Bands);
            }

            OnStateChanged (_page, EngineState.PageFinished);
            OnStateChanged (_page, EngineState.ReportPageFinished);
            EndLastPage();

            //recalculate unlimited
            if (page.UnlimitedHeight || page.UnlimitedWidth)
            {
                PreparedPages.ModifyPageSize (page.Name);
                if (previousPage && _pageNameForRecalc != null)
                {
                    PreparedPages.ModifyPageSize (_pageNameForRecalc);
                }
            }

            //recalculate unlimited
            _page.OnFinishPage (EventArgs.Empty);

            if (_page.BackPage)
            {
                PreparedPages.InterleaveWithBackPage (PreparedPages.CurPage);
            }
        }

        private bool CalcVisibleExpression (string expression)
        {
            var result = true;

            object expressionObj = null;

            // Calculate expressions with TotalPages only on FinalPass.
            if (!expression.Contains ("TotalPages") || (Report.DoublePass && FinalPass))
            {
                expressionObj = Report.Calc (Code.CodeUtils.FixExpressionWithBrackets (expression));
            }

            if (expressionObj is bool obj)
            {
                if (!expression.Contains ("TotalPages"))
                {
                    result = obj;
                }
                else if (FirstPass)
                {
                    result = true;
                }
                else
                {
                    result = obj;
                }
            }

            return result;
        }

        private void RunReportPages()
        {
#if TIMETRIAL
      if (new DateTime($YEAR, $MONTH, $DAY) < System.DateTime.Now)
        throw new Exception("The trial version is now expired!");
#endif

            for (var i = 0; i < Report.Pages.Count; i++)
            {
                var page = Report.Pages[i] as ReportPage;

                // Calc and apply visible expression if needed.
                if (page != null && !string.IsNullOrEmpty (page.VisibleExpression))
                {
                    page.Visible = CalcVisibleExpression (page.VisibleExpression);
                }

                if (page is { Visible: true, Subreport: null })
                {
                    RunReportPage (page);
                }

                if (Report.Aborted)
                {
                    break;
                }
            }
        }

        private void RunBands (BandCollection bands)
        {
            for (var i = 0; i < bands.Count; i++)
            {
                var band = bands[i];
                if (band is DataBand dataBand)
                {
                    RunDataBand (dataBand);
                }
                else if (band is GroupHeaderBand headerBand)
                {
                    RunGroup (headerBand);
                }

                if (Report.Aborted)
                {
                    break;
                }
            }
        }

        private void ShowPageHeader()
        {
            ShowBand (_page.PageHeader);
        }

        private void ShowPageFooter (bool startPage)
        {
            if (!FirstPass &&
                CurPage == TotalPages - 1 &&
                _page.PageFooter != null &&
                (_page.PageFooter.PrintOn & PrintOn.LastPage) > 0 &&
                (_page.PageFooter.PrintOn & PrintOn.FirstPage) == 0 &&
                startPage)
            {
                ShiftLastPage();
            }
            else
            {
                ShowBand (_page.PageFooter);
            }
        }

        private bool StartFirstPage()
        {
            _page.InitializeComponents();

            CurX = 0;
            CurY = 0;
            CurColumn = 0;

            if (_page.ResetPageNumber)
            {
                ResetLogicalPageNumber();
            }

            var previousPage = _page.PrintOnPreviousPage && PreparedPages.Count > 0;

            // check that previous page has the same size
            if (previousPage)
            {
                using (var page0 = PreparedPages.GetPage (PreparedPages.Count - 1))
                {
                    if (page0.PaperWidth == _page.PaperWidth)
                    {
                        if (page0.UnlimitedWidth == _page.UnlimitedWidth)
                        {
                            previousPage = true;
                            if (_page.UnlimitedWidth)
                            {
                                _pageNameForRecalc = page0.Name;
                            }
                        }
                        else
                        {
                            previousPage = false;
                        }
                    }
                    else if (page0.UnlimitedWidth && _page.UnlimitedWidth)
                    {
                        previousPage = true;
                        _pageNameForRecalc = page0.Name;
                    }
                    else
                    {
                        previousPage = false;
                    }

                    if (previousPage)
                    {
                        if (page0.PaperHeight == _page.PaperHeight)
                        {
                            if (page0.UnlimitedHeight == _page.UnlimitedHeight)
                            {
                                previousPage = true;
                                if (_page.UnlimitedHeight)
                                {
                                    _pageNameForRecalc = page0.Name;
                                }
                            }
                            else
                            {
                                previousPage = false;
                            }
                        }
                        else if (page0.UnlimitedHeight && _page.UnlimitedHeight)
                        {
                            previousPage = true;
                        }
                        else
                        {
                            previousPage = false;
                        }
                    }
                }
            }

            // update CurY or add new page
            if (previousPage)
            {
                CurY = PreparedPages.GetLastY();
            }
            else
            {
                PreparedPages.AddPage (_page);
                if (_page.StartOnOddPage && (CurPage % 2) == 1)
                {
                    PreparedPages.AddPage (_page);
                }
            }

            // page numbers
            if (_isFirstReportPage)
            {
                _firstReportPage = CurPage;
            }

            if (_isFirstReportPage && previousPage)
            {
                IncLogicalPageNumber();
            }

            _isFirstReportPage = false;

            OutlineRoot();
            AddPageOutline();

            // show report title and page header
            if (previousPage)
            {
                ShowBand (_page.ReportTitle);
            }
            else
            {
                if (_page.Overlay != null)
                {
                    ShowBand (_page.Overlay);
                }

                if (_page.TitleBeforeHeader)
                {
                    ShowBand (_page.ReportTitle);
                    ShowPageHeader();
                }
                else
                {
                    ShowPageHeader();
                    ShowBand (_page.ReportTitle);
                }
            }

            // show column header
            _columnStartY = CurY;
            ShowBand (_page.ColumnHeader);

            // calculate CurX before starting column event depending on Right to Left or Left to Right layout
            if (Config.RightToLeft)
            {
                CurX = _page.Columns.Positions[_page.Columns.Positions.Count - 1] * Units.Millimeters;
            }
            else
            {
                CurX = 0;
            }

            // start column event
            OnStateChanged (_page, EngineState.ColumnStarted);
            ShowProgress();
            return previousPage;
        }

        private void EndLastPage()
        {
            // end column event
            OnStateChanged (_page, EngineState.ColumnFinished);

            if (_page.ReportSummary != null)
            {
                // do not show column footer here! It's a special case and is handled in the ShowBand.
                ShowBand (_page.ReportSummary);
            }
            else
            {
                ShowBand (_page.ColumnFooter);
            }

            ShowPageFooter (false);
            OutlineRoot();
            _page.FinalizeComponents();
        }

        internal void EndColumn()
        {
            EndColumn (true);
        }

        private void EndColumn (bool showColumnFooter)
        {
            // end column event
            OnStateChanged (_page, EngineState.ColumnFinished);

            // check keep
            if (IsKeeping)
            {
                CutObjects();
            }

            ShowReprintFooters();

            if (showColumnFooter)
            {
                ShowBand (_page.ColumnFooter);
            }

            CurColumn++;

            if (CurColumn >= _page.Columns.Count)
            {
                CurColumn = 0;
            }

            // apply Right to Left layot if needed
            if (Config.RightToLeft)
            {
                _curX = _page.Columns.Positions[_page.Columns.Count - CurColumn - 1] * Units.Millimeters;
            }
            else
            {
                _curX = CurColumn == 0 ? 0 : _page.Columns.Positions[CurColumn] * Units.Millimeters;
            }

            if (CurColumn == 0)
            {
                EndPage();
            }
            else
            {
                StartColumn();
            }

            // end keep
            if (IsKeeping)
            {
                PasteObjects();
            }
        }

        private void StartColumn()
        {
            _curY = _columnStartY;

            ShowBand (_page.ColumnHeader);
            ShowReprintHeaders();

            // start column event
            OnStateChanged (_page, EngineState.ColumnStarted);
        }

        private void EndPage()
        {
            EndPage (true);
        }

        private void StartPage()
        {
            // apply Right to Left layout if needed
            if (Config.RightToLeft)
            {
                CurX = _page.Columns.Positions[_page.Columns.Positions.Count - 1] * Units.Millimeters;
            }
            else
            {
                CurX = 0;
            }

            CurY = 0;
            CurColumn = 0;

            PreparedPages.AddPage (_page);
            AddPageOutline();

            if (_page.Overlay != null)
            {
                ShowBand (_page.Overlay);
            }

            ShowPageHeader();
            OnStateChanged (_page, EngineState.PageStarted);

            _columnStartY = CurY;

            StartColumn();
            ShowProgress();
        }

        #endregion Private Methods

        #region Internal Methods

        internal void EndPage (bool startPage)
        {
            OnStateChanged (_page, EngineState.PageFinished);
            ShowPageFooter (startPage);

            if (_pagesLimit > 0 && PreparedPages.Count >= _pagesLimit)
            {
                Report.Abort();
            }

            if (Report.MaxPages > 0 && PreparedPages.Count >= Report.MaxPages)
            {
                Report.Abort();
            }

            if (startPage)
            {
                StartPage();
            }
        }

        #endregion Internal Methods

        #region Public Methods

        /// <summary>
        /// Starts a new page.
        /// </summary>
        public void StartNewPage()
        {
            EndPage();
        }

        /// <summary>
        /// Starts a new column.
        /// </summary>
        public void StartNewColumn()
        {
            EndColumn();
        }

        #endregion Public Methods
    }
}
