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

        private List<BandBase> _reprintHeaders;
        private List<BandBase> _keepReprintHeaders;
        private List<BandBase> _reprintFooters;
        private List<BandBase> _keepReprintFooters;

        #endregion Fields

        #region Private Methods

        private void InitReprint()
        {
            _reprintHeaders = new List<BandBase>();
            _keepReprintHeaders = new List<BandBase>();
            _reprintFooters = new List<BandBase>();
            _keepReprintFooters = new List<BandBase>();
        }

        private void ShowReprintHeaders()
        {
            var saveOriginX = _originX;

            foreach (var band in _reprintHeaders)
            {
                band.Repeated = true;
                _originX = band.ReprintOffset;
                ShowBand (band);
                band.Repeated = false;
            }

            _originX = saveOriginX;
        }

        private void ShowReprintFooters()
        {
            ShowReprintFooters (true);
        }

        private void ShowReprintFooters (bool repeated)
        {
            var saveOriginX = _originX;

            // show footers in reverse order
            for (var i = _reprintFooters.Count - 1; i >= 0; i--)
            {
                var band = _reprintFooters[i];
                band.Repeated = repeated;
                band.FlagCheckFreeSpace = false;
                _originX = band.ReprintOffset;
                ShowBand (band);
                band.Repeated = false;
                band.FlagCheckFreeSpace = true;
            }

            _originX = saveOriginX;
        }

        private void AddReprint (BandBase band)
        {
            // save current offset and use it later when reprinting a band.
            // it is required when printing subreports
            band.ReprintOffset = _originX;

            if (IsKeeping)
            {
                if (band is DataHeaderBand or GroupHeaderBand)
                {
                    _keepReprintHeaders.Add (band);
                }
                else
                {
                    _keepReprintFooters.Add (band);
                }
            }
            else
            {
                if (band is DataHeaderBand or GroupHeaderBand)
                {
                    _reprintHeaders.Add (band);
                }
                else
                {
                    _reprintFooters.Add (band);
                }
            }
        }

        private void RemoveReprint (BandBase band)
        {
            if (_keepReprintHeaders.Contains (band))
            {
                _keepReprintHeaders.Remove (band);
            }

            if (_reprintHeaders.Contains (band))
            {
                _reprintHeaders.Remove (band);
            }

            if (_keepReprintFooters.Contains (band))
            {
                _keepReprintFooters.Remove (band);
            }

            if (_reprintFooters.Contains (band))
            {
                _reprintFooters.Remove (band);
            }
        }

        private void StartKeepReprint()
        {
            _keepReprintHeaders.Clear();
            _keepReprintFooters.Clear();
        }

        private void EndKeepReprint()
        {
            foreach (var band in _keepReprintHeaders)
            {
                _reprintHeaders.Add (band);
            }

            foreach (var band in _keepReprintFooters)
            {
                _reprintFooters.Add (band);
            }

            _keepReprintHeaders.Clear();
            _keepReprintFooters.Clear();
        }

        private float GetFootersHeight()
        {
            float result = 0;
            var saveRepeated = false;

            foreach (var band in _reprintFooters)
            {
                saveRepeated = band.Repeated;
                band.Repeated = true;
                result += GetBandHeightWithChildren (band);
                band.Repeated = saveRepeated;
            }

            foreach (var band in _keepReprintFooters)
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
