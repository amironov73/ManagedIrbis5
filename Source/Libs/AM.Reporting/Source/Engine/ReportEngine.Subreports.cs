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

using System;

using AM.Reporting.Preview;

#endregion

#nullable enable

namespace AM.Reporting.Engine
{
    public partial class ReportEngine
    {
        #region Private Methods

        private void RenderSubreport (SubreportObject subreport)
        {
            if (subreport.ReportPage != null)
            {
                RunBands (subreport.ReportPage.Bands);
            }
        }

        private void RenderInnerSubreport (BandBase parentBand, SubreportObject subreport)
        {
            var saveOutputBand = outputBand;
            var saveCurX = CurX;
            var saveCurY = CurY;

            try
            {
                outputBand = parentBand;
                CurX = subreport.Left;
                CurY = subreport.Top;

                RenderSubreport (subreport);
            }
            finally
            {
                outputBand = saveOutputBand;
                CurX = saveCurX;
                CurY = saveCurY;
            }
        }

        private void RenderInnerSubreports (BandBase parentBand)
        {
            var originalObjectsCount = parentBand.Objects.Count;
            for (var i = 0; i < originalObjectsCount; i++)
            {
                var subreport = parentBand.Objects[i] as SubreportObject;

                // Apply visible expression if needed.
                if (subreport != null && !string.IsNullOrEmpty (subreport.VisibleExpression))
                {
                    subreport.Visible = CalcVisibleExpression (subreport.VisibleExpression);
                }

                if (subreport != null && subreport.Visible && subreport.PrintOnParent)
                {
                    RenderInnerSubreport (parentBand, subreport);
                }
            }
        }

        private void RenderOuterSubreports (BandBase parentBand)
        {
            var saveCurY = CurY;
            var saveOriginX = originX;
            var saveCurPage = CurPage;

            float maxY = 0;
            var maxPage = CurPage;
            var hasSubreports = false;

            try
            {
                for (var i = 0; i < parentBand.Objects.Count; i++)
                {
                    var subreport = parentBand.Objects[i] as SubreportObject;

                    // Apply visible expression if needed.
                    if (subreport != null && !string.IsNullOrEmpty (subreport.VisibleExpression))
                    {
                        subreport.Visible = CalcVisibleExpression (subreport.VisibleExpression);
                    }

                    if (subreport != null && subreport.Visible && !subreport.PrintOnParent)
                    {
                        hasSubreports = true;

                        // restore start position
                        CurPage = saveCurPage;
                        CurY = saveCurY - subreport.Height;
                        originX = saveOriginX + subreport.Left;

                        // do not upload generated pages to the file cache
                        PreparedPages.CanUploadToCache = false;

                        RenderSubreport (subreport);

                        // find maxY. We will continue from maxY when all subreports finished.
                        if (CurPage == maxPage)
                        {
                            if (CurY > maxY)
                            {
                                maxY = CurY;
                            }
                        }
                        else if (CurPage > maxPage)
                        {
                            maxPage = CurPage;
                            maxY = CurY;
                        }
                    }
                }
            }
            finally
            {
                if (hasSubreports)
                {
                    CurPage = maxPage;
                    CurY = maxY;
                }

                originX = saveOriginX;
                PreparedPages.CanUploadToCache = true;
            }
        }

        #endregion Private Methods
    }
}
