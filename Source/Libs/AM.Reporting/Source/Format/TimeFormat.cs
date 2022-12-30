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
using System.Text;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Format
{
    /// <summary>
    /// Defines how time values are formatted and displayed.
    /// </summary>
    public class TimeFormat : CustomFormat
    {
        #region Public Methods

        /// <inheritdoc/>
        public override FormatBase Clone()
        {
            var result = new TimeFormat
            {
                Format = Format
            };
            return result;
        }

        internal override string GetSampleValue()
        {
            return FormatValue (new DateTime (2007, 11, 30, 13, 30, 0));
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <b>TimeFormat</b> class with default settings.
        /// </summary>
        public TimeFormat()
        {
            Format = "t";
        }
    }
}
