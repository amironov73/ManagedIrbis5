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

#endregion

#nullable enable

namespace AM.Reporting.Format
{
    /// <summary>
    /// Represents a format used to display values with no formatting.
    /// </summary>
    public class GeneralFormat : FormatBase
    {
        #region Public Methods

        /// <inheritdoc/>
        public override FormatBase Clone()
        {
            return new GeneralFormat();
        }

        /// <inheritdoc/>
        public override bool Equals (object obj)
        {
            var f = obj as GeneralFormat;
            return f != null;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc/>
        public override string FormatValue (object value)
        {
            if (value != null)
            {
                return value.ToString();
            }

            return "";
        }

        internal override string GetSampleValue()
        {
            return "";
        }

        #endregion
    }
}
