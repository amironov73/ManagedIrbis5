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

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Reporting.Engine
{
    public partial class ReportEngine
    {
        #region Private Methods

        private void EnumHeaders (BandBase band, List<BandBase> list)
        {
            if (band != null)
            {
                list.Add (band);
                EnumHeaders (band.Parent as BandBase, list);
            }
        }

        // get a list of the footers that must be kept with the dataBand row
        private List<HeaderFooterBandBase> GetAllFooters (DataBand dataBand)
        {
            // get all parent databands/groups
            List<BandBase> list = new List<BandBase>();
            EnumHeaders (dataBand, list);

            // add report summary if required
            var summaryBand = (dataBand.Page as ReportPage).ReportSummary;
            if (dataBand.KeepSummary && summaryBand != null)
            {
                list.Add (summaryBand);
            }

            // make footers list
            List<HeaderFooterBandBase> footers = new List<HeaderFooterBandBase>();
            foreach (var band in list)
            {
                HeaderFooterBandBase footer = null;
                if (band is DataBand band1)
                {
                    footer = band1.Footer;
                }
                else if (band is GroupHeaderBand headerBand)
                {
                    footer = headerBand.GroupFooter;
                }
                else if (band is ReportSummaryBand reportSummaryBand)
                {
                    footer = reportSummaryBand;
                }

                if (footer != null)
                {
                    footers.Add (footer);
                }

                // skip non-last data rows. Keep the dataBand to allow
                // calling this method from the beginning of RunDataBand
                if (band != dataBand && !band.IsLastRow)
                {
                    break;
                }
            }

            // remove all footers at the end which have no KeepWithData flag
            for (var i = footers.Count - 1; i >= 0; i--)
            {
                if (!footers[i].KeepWithData)
                {
                    footers.RemoveAt (i);
                }
                else
                {
                    break;
                }
            }

            return footers;
        }

        private bool NeedKeepFirstRow (DataBand dataBand)
        {
            return dataBand.Header is { KeepWithData: true };
        }

        private bool NeedKeepFirstRow (GroupHeaderBand groupBand)
        {
            if (groupBand == null)
            {
                return false;
            }

            if (groupBand.KeepWithData)
            {
                return true;
            }

            var dataBand = groupBand.GroupDataBand;
            if (dataBand.Header is { KeepWithData: true })
            {
                return true;
            }

            if (groupBand.IsFirstRow)
            {
                return NeedKeepFirstRow (groupBand.Parent as GroupHeaderBand);
            }

            return false;
        }

        private bool NeedKeepLastRow (DataBand dataBand)
        {
            List<HeaderFooterBandBase> footers = GetAllFooters (dataBand);
            return footers.Count > 0;
        }

        private float GetFootersHeight (DataBand dataBand)
        {
            List<HeaderFooterBandBase> footers = GetAllFooters (dataBand);

            float height = 0;
            foreach (var band in footers)
            {
                // skip bands with RepeatOnEveryPage flag: its height is already
                // included in the FreeSpace property
                if (!band.RepeatOnEveryPage)
                {
                    height += GetBandHeightWithChildren (band);
                }
            }

            return height;
        }

        private void CheckKeepFooter (DataBand dataBand)
        {
            if (FreeSpace < GetFootersHeight (dataBand))
            {
                EndColumn();
            }
            else
            {
                EndKeep();
            }
        }

        #endregion Private Methods
    }
}
