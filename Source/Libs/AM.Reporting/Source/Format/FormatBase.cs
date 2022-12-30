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
using System.ComponentModel;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Format
{
    /// <summary>
    /// Base class for all formats.
    /// </summary>
    /// <remarks>
    /// The format is used to format expression value in a <see cref="TextObject"/> object.
    /// </remarks>
    [TypeConverter (typeof (TypeConverters.FormatConverter))]
    public abstract class FormatBase : IFRSerializable
    {
        #region Properties

        /// <summary>
        /// Gets the short format name (e.g. without a "Format" suffix).
        /// </summary>
        [Browsable (false)]
        public string Name => GetType().Name.Replace ("Format", "");

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates exact copy of this format.
        /// </summary>
        /// <returns>The copy of this format.</returns>
        public abstract FormatBase Clone();

        /// <summary>
        /// Formats the specified value.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <returns>The string that represents the formatted value.</returns>
        public abstract string FormatValue (object value);

        internal abstract string GetSampleValue();

        internal virtual void Serialize (FRWriter writer, string prefix, FormatBase format)
        {
            if (format.GetType() != GetType())
            {
                writer.WriteStr ("Format", Name);
            }
        }

        /// <inheritdoc/>
        public void Serialize (FRWriter writer)
        {
            writer.ItemName = GetType().Name;
            Serialize (writer, "", writer.DiffObject as FormatBase);
        }

        /// <inheritdoc/>
        public void Deserialize (FRReader reader)
        {
            reader.ReadProperties (this);
        }

        #endregion
    }
}
