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
using System.Collections;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Table
{
    /// <summary>
    /// Represents a collection of <see cref="TableColumn"/> objects.
    /// </summary>
    public class TableColumnCollection : ReportCollectionBase
    {
        /// <summary>
        /// Gets a column with specified index.
        /// </summary>
        /// <param name="index">Index of a column.</param>
        /// <returns>The column with specified index.</returns>
        public TableColumn this [int index]
        {
            get
            {
                var column = List[index] as TableColumn;
                column.SetIndex (index);
                return column;
            }
        }

        /// <inheritdoc/>
        protected override void OnInsert (int index, object value)
        {
            base.OnInsert (index, value);
            if (Owner != null)
            {
                (Owner as TableBase).CorrectSpansOnColumnChange (index, 1);
            }
        }

        /// <inheritdoc/>
        protected override void OnRemove (int index, object value)
        {
            base.OnRemove (index, value);
            if (Owner != null)
            {
                (Owner as TableBase).CorrectSpansOnColumnChange (index, -1);
            }
        }

        internal TableColumnCollection (Base owner) : base (owner)
        {
        }
    }
}
