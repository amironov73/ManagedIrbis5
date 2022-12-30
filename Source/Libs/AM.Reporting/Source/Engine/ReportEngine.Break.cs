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
using System.Collections.Generic;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Reporting.Engine
{
    public partial class ReportEngine
    {
        #region Private Methods

        private void BreakBand (BandBase band)
        {
            var cloneBand = Activator.CreateInstance (band.GetType()) as BandBase;
            cloneBand.Assign (band);
            cloneBand.SetRunning (true);
            cloneBand.FlagMustBreak = band.FlagMustBreak;

            // clone band objects:
            // - remove bands that can break, convert them to Text objects if necessary
            // - skip subreports
            foreach (Base c in band.Objects)
            {
                if (c is BandBase @base && @base.CanBreak)
                {
                    if (@base.HasBorder || @base.HasFill)
                    {
                        var textObj = new TextObject();
                        textObj.Bounds = @base.Bounds;
                        textObj.Border = @base.Border.Clone();
                        textObj.Fill = @base.Fill.Clone();
                        cloneBand.Objects.Add (textObj);
                    }

                    foreach (ReportComponentBase obj in @base.Objects)
                    {
                        if (!(obj is BandBase))
                        {
                            var cloneObj =
                                Activator.CreateInstance (obj.GetType()) as ReportComponentBase;
                            cloneObj.AssignAll (obj);
                            cloneObj.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                            cloneObj.Dock = DockStyle.None;
                            cloneObj.Left = obj.AbsLeft - band.AbsLeft;
                            cloneObj.Top = obj.AbsTop - band.AbsTop;
                            if (cloneObj is TextObject textObject)
                            {
                                textObject.Highlight.Clear();
                            }

                            cloneBand.Objects.Add (cloneObj);
                        }
                    }
                }
                else if (!(c is SubreportObject))
                {
                    var cloneObj = Activator.CreateInstance (c.GetType()) as Base;
                    cloneObj.AssignAll (c);
                    cloneObj.Parent = cloneBand;
                }
            }

            var breakTo = Activator.CreateInstance (band.GetType()) as BandBase;
            breakTo.Assign (band);
            breakTo.SetRunning (true);
            breakTo.Child = null;
            breakTo.CanGrow = true;
            breakTo.StartNewPage = false;
            breakTo.OutlineExpression = "";
            breakTo.BeforePrintEvent = "";
            breakTo.BeforeLayoutEvent = "";
            breakTo.AfterPrintEvent = "";
            breakTo.AfterLayoutEvent = "";

            // breakTo must be breaked because it will print on a new page.
            breakTo.FlagMustBreak = true;

            // to allow clone and breaked bands to access Report
            cloneBand.SetReport (Report);
            breakTo.SetReport (Report);

            try
            {
                // (case: object with Anchor = bottom on a breakable band)
                // disable re-layout
                cloneBand.SetUpdatingLayout (true);
                cloneBand.Height = FreeSpace;
                cloneBand.SetUpdatingLayout (false);

                if (cloneBand.Break (breakTo))
                {
                    AddToPreparedPages (cloneBand);
                    EndColumn();

                    // CalcHeight fixes the height of objects in the remaining part
                    breakTo.CalcHeight();
                    AddToPreparedPages (breakTo);
                }
                else
                {
                    if (cloneBand.FlagMustBreak)
                    {
                        // show band as is
                        breakTo.FlagCheckFreeSpace = false;
                        AddToPreparedPages (breakTo);
                    }
                    else
                    {
                        EndColumn();
                        breakTo.CalcHeight();
                        AddToPreparedPages (breakTo);
                    }
                }
            }
            finally
            {
                cloneBand.Dispose();
                breakTo.Dispose();
            }
        }

        private bool BandHasHardPageBreaks (BandBase band)
        {
            foreach (var obj in band.Objects)
            {
                if ((obj as ReportComponentBase).PageBreak)
                {
                    return true;
                }
            }

            return false;
        }

        private BandBase[] SplitHardPageBreaks (BandBase band)
        {
            List<BandBase> parts = new List<BandBase>();

            BandBase cloneBand = null;
            float offsetY = 0;

            foreach (ReportComponentBase c in band.Objects)
            {
                if (c.PageBreak)
                {
                    if (cloneBand != null)
                    {
                        cloneBand.Height = c.Top - offsetY;
                    }

                    cloneBand = null;
                    offsetY = c.Top;
                }

                if (cloneBand == null)
                {
                    cloneBand = Activator.CreateInstance (band.GetType()) as BandBase;
                    cloneBand.Assign (band);
                    cloneBand.SetRunning (true);
                    if (c.PageBreak)
                    {
                        cloneBand.StartNewPage = true;
                        cloneBand.FirstRowStartsNewPage = true;
                    }

                    parts.Add (cloneBand);
                }

                var cloneObj = Activator.CreateInstance (c.GetType()) as ReportComponentBase;
                cloneObj.AssignAll (c);
                cloneObj.Top = c.Top - offsetY;
                cloneObj.Parent = cloneBand;
            }

            if (cloneBand != null)
            {
                cloneBand.Height = band.Height - offsetY;
            }

            return parts.ToArray();
        }

        #endregion Private Methods
    }
}
