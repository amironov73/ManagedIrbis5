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

using AM.Reporting.Table;
using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Matrix
{
    /// <summary>
    /// The base class for matrix element descriptors such as <see cref="MatrixHeaderDescriptor"/> and
    /// <see cref="MatrixCellDescriptor"/>.
    /// </summary>
    public class MatrixDescriptor : IFRSerializable
    {
        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets an expression which value will be used to fill the matrix.
        /// </summary>
        /// <remarks>
        /// <b>Expression</b> may be any valid expression. Usually it's a data column:
        /// <c>[DataSource.Column]</c>.
        /// </remarks>
        public string Expression { get; set; }

        /// <summary>
        /// Gets or sets the template column bound to this descriptor.
        /// </summary>
        /// <remarks>
        /// This property is for internal use; usually you don't need to use it.
        /// </remarks>
        public TableColumn TemplateColumn { get; set; }

        /// <summary>
        /// Gets or sets the template row bound to this descriptor.
        /// </summary>
        /// <remarks>
        /// This property is for internal use; usually you don't need to use it.
        /// </remarks>
        public TableRow TemplateRow { get; set; }

        /// <summary>
        /// Gets or sets the template cell bound to this descriptor.
        /// </summary>
        /// <remarks>
        /// Using this property, you may access the matrix cell which is bound to
        /// this descriptor. It may be useful to change the cell's appearance.
        /// <note>
        /// Before using this property, you must initialize the matrix descriptors by
        /// calling the <see cref="MatrixObject.BuildTemplate"/> method.
        /// </note>
        /// </remarks>
        /// <example>
        /// <code>
        /// MatrixObject matrix;
        /// // change the fill color of the first matrix cell
        /// matrix.Data.Cells[0].TemplateCell.Fill = new SolidFill(Color.Red);
        /// </code>
        /// </example>
        public TableCell TemplateCell { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Assigns values from another descriptor.
        /// </summary>
        /// <param name="source">Descriptor to assign values from.</param>
        public virtual void Assign (MatrixDescriptor source)
        {
            Expression = source.Expression;
            TemplateCell = source.TemplateCell;
        }

        /// <inheritdoc/>
        public virtual void Serialize (ReportWriter writer)
        {
            var c = writer.DiffObject as MatrixDescriptor;

            if (Expression != c.Expression)
            {
                writer.WriteStr ("Expression", Expression);
            }
        }

        /// <inheritdoc/>
        public void Deserialize (FRReader reader)
        {
            reader.ReadProperties (this);
        }

        #endregion
    }
}
