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
using System.Drawing;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting
{
    /// <summary>
    /// Represents a style.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Style class holds border, fill, text fill and font settings. It can be applied to any report object of
    /// <see cref="ReportComponentBase"/> type.
    /// </para>
    /// <para>
    /// The <b>Report</b> object holds list of styles in its <see cref="Report.Styles"/> property. Each style has
    /// unique name. To apply a style to the report component, set its <see cref="ReportComponentBase.Style"/>
    /// property to the style name.
    /// </para>
    /// </remarks>
    public class Style : StyleBase
    {
        /// <summary>
        /// Gets or sets a name of the style.
        /// </summary>
        /// <remarks>
        /// The name must be unique.
        /// </remarks>
        public string Name { get; set; }

        /// <inheritdoc/>
        public override void Serialize (FRWriter writer)
        {
            writer.ItemName = "Style";
            writer.WriteStr ("Name", Name);
            base.Serialize (writer);
        }

        /// <inheritdoc/>
        public override void Assign (StyleBase source)
        {
            base.Assign (source);
            Name = (source as Style).Name;
        }

        /// <summary>
        /// Creates exact copy of this <b>Style</b>.
        /// </summary>
        /// <returns>Copy of this style.</returns>
        public Style Clone()
        {
            var result = new Style();
            result.Assign (this);
            return result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Style"/> class with default settings.
        /// </summary>
        public Style()
        {
            Name = "";
            ApplyBorder = true;
            ApplyFill = true;
            ApplyTextFill = true;
            ApplyFont = true;
        }
    }
}
