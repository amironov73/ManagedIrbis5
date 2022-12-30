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
        #region Fields

        private List<BandBase> reprintHeaders;
        private List<BandBase> keepReprintHeaders;
        private List<BandBase> reprintFooters;
        private List<BandBase> keepReprintFooters;

        #endregion Fields

        #region Private Methods

        private void InitReprint()
        {
            reprintHeaders = new List<BandBase>();
            keepReprintHeaders = new List<BandBase>();
            reprintFooters = new List<BandBase>();
            keepReprintFooters = new List<BandBase>();
        }

        private void ShowReprintHeaders()
        {
            var saveOriginX = originX;

            foreach (var band in reprintHeaders)
            {
                band.Repeated = true;
                originX = band.ReprintOffset;
                ShowBand (band);
                band.Repeated = false;
            }

            originX = saveOriginX;
        }

        private void ShowReprintFooters()
        {
            ShowReprintFooters (true);
        }

        private void ShowReprintFooters (bool repeated)
        {
            var saveOriginX = originX;

            // show footers in reverse order
            for (var i = reprintFooters.Count - 1; i >= 0; i--)
            {
                var band = reprintFooters[i];
                band.Repeated = repeated;
                band.FlagCheckFreeSpace = false;
                originX = band.ReprintOffset;
                ShowBand (band);
                band.Repeated = false;
                band.FlagCheckFreeSpace = true;
            }

            originX = saveOriginX;
        }

        private void AddReprint (BandBase band)
        {
            // save current offset and use it later when reprinting a band.
            // it is required when printing subreports
            band.ReprintOffset = originX;

            if (IsKeeping)
            {
                if (band is DataHeaderBand || band is GroupHeaderBand)
                {
                    keepReprintHeaders.Add (band);
                }
                else
                {
                    keepReprintFooters.Add (band);
                }
            }
            else
            {
                if (band is DataHeaderBand || band is GroupHeaderBand)
                {
                    reprintHeaders.Add (band);
                }
                else
                {
                    reprintFooters.Add (band);
                }
            }
        }

        private void RemoveReprint (BandBase band)
        {
            if (keepReprintHeaders.Contains (band))
            {
                keepReprintHeaders.Remove (band);
            }

            if (reprintHeaders.Contains (band))
            {
                reprintHeaders.Remove (band);
            }

            if (keepReprintFooters.Contains (band))
            {
                keepReprintFooters.Remove (band);
            }

            if (reprintFooters.Contains (band))
            {
                reprintFooters.Remove (band);
            }
        }

        private void StartKeepReprint()
        {
            keepReprintHeaders.Clear();
            keepReprintFooters.Clear();
        }

        private void EndKeepReprint()
        {
            foreach (var band in keepReprintHeaders)
            {
                reprintHeaders.Add (band);
            }

            foreach (var band in keepReprintFooters)
            {
                reprintFooters.Add (band);
            }

            keepReprintHeaders.Clear();
            keepReprintFooters.Clear();
        }

        private float GetFootersHeight()
        {
            float result = 0;
            var saveRepeated = false;

            foreach (var band in reprintFooters)
            {
                saveRepeated = band.Repeated;
                band.Repeated = true;
                result += GetBandHeightWithChildren (band);
                band.Repeated = saveRepeated;
            }

            foreach (var band in keepReprintFooters)
            {
                saveRepeated = band.Repeated;
                band.Repeated = true;
                result += GetBandHeightWithChildren (band);
                band.Repeated = saveRepeated;
            }

            return result;
        }

        #endregion Private Methods
    }
}
